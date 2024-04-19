using EnergyEfficiencyBE.Models.Auth;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;

namespace EnergyEfficiencyBE.Services
{
    public interface IJwtService
    {
        Task<Result<TokenModel>> GenerateTokenPairAsync(AuthUser user);
        Task<Result<TokenModel>> RefreshTokenAsync(RefreshTokenModel tokenModel);
    }
}
