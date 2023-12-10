using DC.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DC.Core.Data.Context
{
    public static class ModelBuilderExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder)
        {
            Expression<Func<IAudityEntity, bool>> softDeleteFilter = u => !u.IsDeleted;

            SetQueryFilter(modelBuilder, softDeleteFilter);
		}
        public static void SetQueryFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> filter) where T : class
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(T).IsAssignableFrom(entityType.ClrType))
                {
                    ParameterExpression? parameter = Expression.Parameter(entityType.ClrType);
                    InvocationExpression? access = Expression.Invoke(filter, parameter);
                    LambdaExpression? lambda = Expression.Lambda(access, parameter);

                    entityType.SetQueryFilter(lambda);
                }
            }
        }

    }
}
