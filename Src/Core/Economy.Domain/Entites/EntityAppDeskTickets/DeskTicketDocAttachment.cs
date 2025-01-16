using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
	public class DeskTicketDocAttachment : BaseEntity<int>
    {
        public DeskTicketDocAttachment() { }
        public int? DeskTicketId { get; set; } 
        public DeskTicket? DeskTicket { get; set; } 
        public int? AttachmentDocumentId { get; set; }
    }
}
