using DC.Core.Data.DynamicQuery;
using DC.Core.Data.Paging;
using DC.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace DC.Core.Data.Repository
{
    public class GenericRepository<TEntity> : IEntityRepository<TEntity> 
        where TEntity : class, IEntity
    {
        protected DbContext Context { get; }

        public GenericRepository(DbContext context)
        {
            Context = context;
        }

        private IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>();
        }

        public async Task<TEntity?> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }


        public async Task<IPaginate<TEntity>> GetList(Expression<Func<TEntity, bool>>? predicate = null,
                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                                       int index = 0, int size = 10, bool enableTracking = true,
                                                       CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query();

            if (!enableTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include(queryable);

            if (predicate != null) queryable = queryable.Where(predicate);

            if (orderBy != null)
                return await orderBy(queryable).ToPaginateAsync(index, size, 0, cancellationToken);

            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }

        public async Task<IPaginate<TEntity>> GetListByDynamic(Dynamic dynamic,
                                                                    Func<IQueryable<TEntity>,IIncludableQueryable<TEntity, object>>? include = null,
                                                                    int index = 0, int size = 10, bool enableTracking = true,
                                                                    CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> queryable = Query().AsQueryable().ToDynamic(dynamic);

            if (!enableTracking) queryable = queryable.AsNoTracking();

            if (include != null) queryable = include(queryable);

            return await queryable.ToPaginateAsync(index, size, 0, cancellationToken);
        }


        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null) return Query().Where(predicate);

            return Query();
        }

        public async Task Add(TEntity entity)
        {
            await Context.AddAsync(entity);
        }

        public async Task Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Deleted;
        }

        public async Task DeleteById(int id)
        {
            TEntity? entityToDelete = await Context.Set<TEntity>().FindAsync(id);

            if (entityToDelete is null)
                throw new Exception("Entity not found");

            Context.Entry(entityToDelete).State = EntityState.Deleted;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task ExecuteSQL(string query)
        {
            await Context.Database.ExecuteSqlRawAsync(query);
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Context.Database.BeginTransactionAsync();
        }

        public async Task CreateExecutionStrategyAsync(Func<Task> operation)
        {
            IExecutionStrategy strategy = Context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(operation);
        }

        public TContext GetContext<TContext>() where TContext : DbContext
        {
            return (TContext)Context;
        }
    }
}
