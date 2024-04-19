using EnergyEfficiencyBE.Context;
using EnergyEfficiencyBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnergyEfficiencyBE.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly RelationalContext _context;

        public UserRepository(RelationalContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<User?> FindByIdAsync(Guid id)
        => await DbSet.FirstOrDefaultAsync(u => u.Id == id);

        public override async Task<List<User>?> FindAllAsync()
            => await DbSet.ToListAsync();

        public override async Task<List<User>?> FindByConditionAsync(Expression<Func<User, bool>> expression) =>
            await DbSet.AsNoTracking().Where(expression).ToListAsync();

        public override async Task DeleteAsync(User entity)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DbSet.Remove(entity);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
