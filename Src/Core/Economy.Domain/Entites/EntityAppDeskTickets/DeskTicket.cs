using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppDeskTickets
{
	public class DeskTicket : BaseEntity<string>
    {
        public DeskTicket() { }
        public string? Number { get; set; } = string.Empty;
        public string? Title { get; set; } = string.Empty;
        public string? TicketText { get; set; } = string.Empty;
       
        public DateTime TicketDate { get; set; } = DateTime.Now;
        public DateTime TargetResolutionDate { get; set; } = DateTime.Now.AddMonths(1);
        public DateTime? ActualResolutionDate { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Draft;
        public TicketPriority Priority { get; set; } = TicketPriority.Low;
        public ICollection<DeskTicketImageAttachment> DeskTicketImageAttachments { get; set; } = new List<DeskTicketImageAttachment>();
        public ICollection<DeskTicketDocAttachment> DeskTicketDocAttachments { get; set; } = new List<DeskTicketDocAttachment>();
        public ICollection<DeskTicketThread> DeskTicketThreads { get; set; } = new List<DeskTicketThread>();
    }
}
