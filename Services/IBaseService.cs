using EnergyEfficiencyBE.Dtos;
using EnergyEfficiencyBE.Models.ResultPattern;
using System.Linq.Expressions;

namespace EnergyEfficiencyBE.Services
{
    public interface IBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : BaseDto
    {
        Task<Result<List<TEntity>?>> FindAllAsync();
        Task<Result<TEntity?>> FindByIdAsync(Guid id);
        Task<Result<List<TEntity>?>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
        Task<Result> CreateAsync(TDto dto);
        Task<Result> UpdateAsync(TDto dto);
        Task<Result> DeleteAsync(Guid id);
    }
}
