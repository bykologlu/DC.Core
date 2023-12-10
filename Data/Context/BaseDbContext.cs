using DC.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace DC.Core.Data.Context
{
    public abstract class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        private readonly ILogger<BaseDbContext> _logger;

        private readonly List<Action<ModelBuilder>> _modelCreatingActions = new List<Action<ModelBuilder>>();

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration, params Action<ModelBuilder>[] modelCreatingActions) : base(dbContextOptions)
        {
            if (modelCreatingActions != null) _modelCreatingActions = modelCreatingActions.ToList();

            Configuration = configuration;
        }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration, ILogger<BaseDbContext> logger) : this(dbContextOptions,configuration)
        {
            _logger = logger;
        }

        public void AddModelCreatingAction(Action<ModelBuilder> action)
        {
            _modelCreatingActions.Add(action);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var action in _modelCreatingActions)
            {
                action(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                OnBeforeSaving();

                int result = base.SaveChanges(acceptAllChangesOnSuccess);

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "DB save changes exception");
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(
           bool acceptAllChangesOnSuccess,
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            try
            {
                OnBeforeSaving();

                int result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "DB save changes exception");
                throw;
            }
        }

        private void OnBeforeSaving()
        {
            IEnumerable<EntityEntry> entries = ChangeTracker.Entries();

            DateTime now = DateTime.Now;

            foreach (EntityEntry entry in entries)
            {
                if (entry.Entity is IAudityEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.UpdatedDate = now;
                            entry.Property("CreatedDate").IsModified = false;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            trackable.UpdatedDate = now;
                            trackable.IsDeleted = true;
                            entry.Property("CreatedDate").IsModified = false;
                            break;
                        case EntityState.Added:
                            trackable.CreatedDate = now;
                            break;

                    }
                }
            }
        }

    }
}
