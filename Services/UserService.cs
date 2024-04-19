using AutoMapper;
using EnergyEfficiencyBE.Dtos;
using EnergyEfficiencyBE.Models.Entities;
using EnergyEfficiencyBE.Models.ResultPattern;
using EnergyEfficiencyBE.Repositories;
using Microsoft.AspNetCore.Identity;

namespace EnergyEfficiencyBE.Services
{
    public class UserService : BaseService<User, UserBaseDto>, IUserService
    {
        private readonly UserManager<AuthUser> _userManager;

        public UserService(IUserRepository repository, IMapper mapper, UserManager<AuthUser> userManager) : base(repository,
            mapper)
        {
            _userManager = userManager;
        }

        public new async Task<Result> UpdateAsync(UserUpdateDto dto)
        {
            try
            {
                var user = await Repository.FindByIdAsync(dto.Id);
                var authUser = await _userManager.FindByIdAsync(user.IdentityId);

                Mapper.Map(dto, user);
                authUser.UserName = user.Name;

                await Repository.UpdateAsync(user);
                await _userManager.UpdateAsync(authUser);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("UserService Server Fail!");
            }
        }

        public override async Task<Result> DeleteAsync(Guid applicationUserId)
        {
            try
            {
                var applicationUser = await Repository.FindByIdAsync(applicationUserId);
                var userName = applicationUser.Name;
                var identityUser = await _userManager.FindByIdAsync(applicationUser.IdentityId);

                if (applicationUser is null) return Result.Fail("User in application doesn't exist.");

                if (identityUser is null) return Result.Fail("User in identity doesn't exist.");

                await Repository.DeleteAsync(applicationUser);
                await _userManager.DeleteAsync(identityUser);

                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("UserService Server Fail!");
            }
        }
    }
}
