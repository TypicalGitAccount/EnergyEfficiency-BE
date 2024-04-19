using EnergyEfficiencyBE.Models.Auth;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnergyEfficiencyBE.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly JwtBearerOptions _jwtBearerOptions;
        private readonly UserManager<AuthUser> _userManager;
        private readonly IUserService _userService;

        public JwtService(IOptions<JwtOptions> jwtSettings, UserManager<AuthUser> userManager,
            IOptions<JwtBearerOptions> jwtBearerOptions, IUserService userService)
        {
            _jwtOptions = jwtSettings.Value;
            _userManager = userManager;
            _jwtBearerOptions = jwtBearerOptions.Value;
            _userService = userService;
        }

        public async Task<Result<TokenModel>> GenerateTokenPairAsync(AuthUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = await GenerateUserClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(Constants.AccessTokenLifetimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience
            };

            var securityToken = tokenHandler.CreateToken(descriptor);

            var accessToken = tokenHandler.WriteToken(securityToken);

            var refreshToken = GenerateRefreshToken();

            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(Constants.RefreshTokenLifetimeInMinutes);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);

            return Result.Ok(new TokenModel()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        public async Task<Result<TokenModel>> RefreshTokenAsync(RefreshTokenModel tokenModel)
        {
            var principal = GetPrincipalFromExpiredToken(tokenModel.AccessToken);

            if (principal?.FindFirstValue(ClaimTypes.Email) is null)
            {
                return Result.Fail<TokenModel>("The provided token is not valid!");
            }

            var user = await _userManager.FindByEmailAsync(principal.FindFirstValue(ClaimTypes.Email)!);

            if (user is null)
            {
                return Result.Fail<TokenModel>(
                    $"The user with email {principal.FindFirstValue(ClaimTypes.Email)!} was not found!");
            }

            if (user.RefreshToken != tokenModel.RefreshToken)
                return Result.Fail<TokenModel>("The provided refresh token is not valid.");

            if (user.RefreshTokenExpiryTime <= DateTime.Now)
                return Result.Fail<TokenModel>("The provided refresh token is expired.");

            return await GenerateTokenPairAsync(user);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private async Task<List<Claim>> GenerateUserClaims(AuthUser user)
        {
            var applicationUser = (await _userService.FindByConditionAsync(
                u => u.IdentityId == user.Id)).Value.FirstOrDefault();
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new("uid", applicationUser.Id.ToString())
        };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var principal = tokenHandler.ValidateToken(token,
                _jwtBearerOptions.TokenValidationParameters,
                out var securityToken);

            return CheckSecurityToken(securityToken) ? principal : null;
        }

        private static bool CheckSecurityToken(SecurityToken securityToken) =>
            securityToken is JwtSecurityToken jwtSecurityToken &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
    }
}
