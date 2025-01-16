using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
	public class DeskTicketThread : BaseEntity<int>
    {
        public DeskTicketThread() { }
        public int? DeskTicketId { get; set; } 
        public DeskTicket? DeskTicket { get; set; } 
        public string? DiscussionText { get; set; } = string.Empty;
        public DeskTicketThreadCreatorType CreatorType { get; set; } = DeskTicketThreadCreatorType.Agent;
        public ICollection<DeskTicketThreadImageAttachment> DeskTicketThreadImageAttachments { get; set; } = new List<DeskTicketThreadImageAttachment>();
        public ICollection<DeskTicketThreadDocAttachment> DeskTicketThreadDocAttachments { get; set; } = new List<DeskTicketThreadDocAttachment>();
    }
}
