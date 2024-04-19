using EnergyEfficiencyBE.Models.Auth;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;
using EnergyEfficiencyBE.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EnergyEfficiencyBE.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly UserManager<AuthUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IJwtService tokenService,
            SignInManager<AuthUser> signInManager,
            UserManager<AuthUser> userManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _jwtService = tokenService;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Result<(IdentityResult, User)>> RegisterUserAsync(RegisterModel model)
        {
            string userId = String.Empty;
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                    return Result.Fail<(IdentityResult, User)>(
                        $"User with {model.Email} already exists!");

                user = new AuthUser { UserName = model.Name, Email = model.Email };
                userId = user.Id;

                var identityResult = await _userManager.CreateAsync(user, model.Password);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    var applicationResult = await CreateApplicationUser(user);

                    return applicationResult.IsSuccess
                        ? Result.Ok((identityResult, applicationResult.Value))
                        : Result.Fail<(IdentityResult, User)>(applicationResult.Message);
                }
                else
                {
                    return Result.Fail<(IdentityResult, User)>(
                        $"Register failed: {identityResult.Errors.FirstOrDefault().Description}");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<(IdentityResult, User)>(
                    $"Register failed!"
                );
            }
        }

        public async Task<Result<TokenModel>> LoginUserAsync(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null) return Result.Fail<TokenModel>($"The user with {model.Email} doesn't exist!");

                var signInResult = await _signInManager.PasswordSignInAsync(
                    user: user,
                    password: model.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

                return signInResult.Succeeded
                    ? await _jwtService.GenerateTokenPairAsync(user)
                    : Result.Fail<TokenModel>($"Invalid password!");
            }
            catch (Exception ex)
            {
                return Result.Fail<TokenModel>($"Login failed!");
            }
        }

        public async Task<Result<TokenModel>> ChangePasswordAsync(ChangePasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null) return Result.Fail<TokenModel>($"The user with {model.Email} doesn't exist!");

                var passwordCheck =
                    _userManager.PasswordHasher.VerifyHashedPassword(user!, user.PasswordHash!, model.OldPassword);

                if (passwordCheck is PasswordVerificationResult.Failed)
                    return Result.Fail<TokenModel>($"The old password is not correct!");

                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded) return await _jwtService.GenerateTokenPairAsync(user);
                else return Result.Fail<TokenModel>("Invalid change password attempt!");
            }
            catch (Exception ex)
            {
                return Result.Fail<TokenModel>(
                    $"Change password failed!"
                );
            }
        }

        private async Task<Result<User>> CreateApplicationUser(AuthUser user)
        {
            try
            {
                var entity = (await _userRepository.FindByConditionAsync(u => u.IdentityId == user.Id))
                    .FirstOrDefault();
                if (entity is null)
                {
                    var applicationUser = new User()
                    {
                        Name = user.UserName,
                        Email = user.Email,
                        IdentityId = user.Id
                    };

                    await _userRepository.CreateAsync(applicationUser);

                    return Result.Ok(applicationUser);
                }
                else
                {
                    return Result.Fail<User>($"User with email{entity.Email} already exists!");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<User>($"AuthService Server Fail!");
            }
        }
    }
}
