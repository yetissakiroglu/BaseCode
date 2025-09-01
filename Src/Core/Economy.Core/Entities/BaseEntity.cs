using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.BaseEntities
{
    public abstract class BaseEntity<TId> : ISoftDelete, IHasId<TId>
    {
		[Key]
        public TId Id { get; set; } = default!;
		public bool IsDeleted { get; set; } = default!;
      
        protected BaseEntity()
		{
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
    
    // Audit log için tarih interface'i
    public interface IHasAuditDates
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        DateTime? DeletedAt { get; set; }
    }

}





