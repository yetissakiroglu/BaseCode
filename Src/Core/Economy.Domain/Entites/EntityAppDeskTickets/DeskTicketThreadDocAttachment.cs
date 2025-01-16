using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
    public class DeskTicketThreadDocAttachment : BaseEntity<int>
    {
        public DeskTicketThreadDocAttachment() { }
        public int? DeskTicketThreadId { get; set; } 
        public DeskTicketThread? DeskTicketThread { get; set; } 
        public int? AttachmentDocumentId { get; set; } 
    }
}
