namespace DC.Core.Entity.Base
{
    public class AudityEntity<T> : BaseEntity<T>, IAudityEntity
	{
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AudityEntity : AudityEntity<int>
    {

    }
}
