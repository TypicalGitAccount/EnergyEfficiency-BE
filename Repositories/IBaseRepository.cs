using System.Linq.Expressions;

namespace EnergyEfficiencyBE.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<List<T>?> FindAllAsync();
        Task<T?> FindByIdAsync(Guid id);
        Task<List<T>?> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
