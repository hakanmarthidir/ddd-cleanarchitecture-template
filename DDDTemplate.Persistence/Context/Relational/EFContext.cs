using Microsoft.EntityFrameworkCore;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;

namespace DDDTemplate.Persistence.Context.Relational
{
    public sealed class EFContext : DbContext
    {
        public DbSet<User> User { get; set; }

        public EFContext(DbContextOptions<EFContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}