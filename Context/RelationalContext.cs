using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnergyEfficiencyBE.Context
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions<RelationalContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}