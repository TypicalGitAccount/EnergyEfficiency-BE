using EnergyEfficiencyBE.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnergyEfficiencyBE.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(RelationalContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task<List<T>?> FindAllAsync() => await DbSet.ToListAsync();

        public virtual async Task<T?> FindByIdAsync(Guid id) => await DbSet.FindAsync(id);


        public virtual async Task<List<T>?> FindByConditionAsync(Expression<Func<T, bool>> expression) =>
            await DbSet.AsNoTracking().Where(expression).ToListAsync();

        public virtual async Task CreateAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }
    }
}
