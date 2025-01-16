namespace Economy.Domain.BaseEntities
{
    public abstract class BaseEntity<TId> : ISoftDelete, IHasId<TId>
	{
		public TId Id { get; set; }
		public string CreatedBy { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
		public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
		public bool IsDeleted { get; set; }

        // Constructor to set default values
        protected BaseEntity()
		{
            CreatedAt = DateTime.Now;
            IsDeleted = false;
		}
	}

	public interface ISoftDelete
	{
		bool IsDeleted { get; set; }
	}

	public interface IHasId<TId>
	{
		 TId Id { get; set; }
	}

}





