using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DC.Core.Entities.Base
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
