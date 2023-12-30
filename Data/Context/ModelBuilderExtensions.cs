using DC.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

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
                    var newParam = Expression.Parameter(entityType.ClrType);
                    var newBody = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), newParam, filter.Body);
                    var newLambda = Expression.Lambda(newBody, newParam);

                    entityType.SetQueryFilter(newLambda);
                }
            }
        }

    }
}
