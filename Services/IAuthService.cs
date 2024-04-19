using EnergyEfficiencyBE.Models.Auth;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;
using Microsoft.AspNetCore.Identity;

namespace EnergyEfficiencyBE.Services
{
    public interface IAuthService
    {
        Task<Result<(IdentityResult, User)>> RegisterUserAsync(RegisterModel model);
        Task<Result<TokenModel>> LoginUserAsync(LoginModel model);
        Task<Result<TokenModel>> ChangePasswordAsync(ChangePasswordModel model);
    }
}
