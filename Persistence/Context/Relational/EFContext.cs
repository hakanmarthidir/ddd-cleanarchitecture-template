using Domain.Entities.UserAggregate;
using Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context.Relational
{
    public sealed class EFContext : DbContext
    {
        private readonly IMediator? _mediator;
        public DbSet<User> User { get; set; }

        public EFContext(DbContextOptions<EFContext> options, IMediator? mediator)
        : base(options)
        {
            this._mediator = mediator;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_mediator == null) return result;

            await this.DispatchEvents();

            return result;
        }        

        private async Task DispatchEvents()
        {
            var entityWithRaisedEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e._domainEvents.Any())
                .ToList();

            foreach (var entity in entityWithRaisedEvents)
            {
                var raisedEvents = entity._domainEvents.ToList();
                entity.ClearEvents();
                foreach (var domainEvent in raisedEvents)
                {
                    await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
                
            }
        }
    }
}