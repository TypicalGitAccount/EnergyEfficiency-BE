using EnergyEfficiencyBE.Dtos;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;

namespace EnergyEfficiencyBE.Services
{
    public interface IUserService : IBaseService<User, UserBaseDto>
    {
        Task<Result> DeleteAsync(Guid applicationUserId);
        Task<Result> UpdateAsync(UserUpdateDto dto);
    }
}
