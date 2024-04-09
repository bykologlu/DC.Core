using System.ComponentModel.DataAnnotations;

namespace DC.Core.Entity.Base
{
    public class BaseEntity<T> : IEntity
    {
        [Key]
        public virtual T Id { get; set; }

    }

    public class BaseEntity : BaseEntity<int>
    {
    }
}
