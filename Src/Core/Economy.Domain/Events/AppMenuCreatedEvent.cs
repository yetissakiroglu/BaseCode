using Economy.Domain.Common;

namespace Economy.Domain.Events
{
    public class AppMenuCreatedEvent : IDomainEvent
    {
        public AppMenuCreatedEvent(int menuId, string title, string slug, string createdBy)
        {
            MenuId = menuId;
            Title = title;
            Slug = slug;
            CreatedBy = createdBy;
            OccurredOn = DateTime.UtcNow;
        }

        public int MenuId { get; }
        public string Title { get; }
        public string Slug { get; }
        public string CreatedBy { get; }
        public DateTime OccurredOn { get; }
    }
}
