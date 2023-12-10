namespace DC.Core.Entities.Base
{
	public interface IAudityEntity
	{
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public bool IsDeleted { get; set; }
	}
}
