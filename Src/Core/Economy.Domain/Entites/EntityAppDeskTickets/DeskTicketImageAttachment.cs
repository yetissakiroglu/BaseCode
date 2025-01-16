using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
	public class DeskTicketImageAttachment : BaseEntity<int>
    {
        public DeskTicketImageAttachment() { }
        public int? DeskTicketId { get; set; } 
        public DeskTicket? DeskTicket { get; set; } 
        public int? AttachmentImageId { get; set; } 
    }
}
