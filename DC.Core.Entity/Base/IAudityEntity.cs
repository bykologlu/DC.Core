namespace DC.Core.Entity.Base
{
	public interface IAudityEntity
	{
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool IsDeleted { get; set; }
	}
}
