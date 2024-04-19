using EnergyEfficiencyBE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EnergyEfficiencyBE.Context
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions<RelationalContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            ConfigureUser(modelBuilder);
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.IdentityId).IsRequired();

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(255);
                entity.Property(e => e.Telegram).HasMaxLength(255);
            });
        }
    }
}