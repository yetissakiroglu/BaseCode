using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
	public class DeskTicketThreadImageAttachment : BaseEntity<int>
    {
        public DeskTicketThreadImageAttachment() { }
        public int? DeskTicketThreadId { get; set; } 
        public DeskTicketThread? DeskTicketThread { get; set; } 
        public int? AttachmentImageId { get; set; } 
    }
}
