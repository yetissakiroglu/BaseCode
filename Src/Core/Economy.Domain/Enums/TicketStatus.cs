using System.ComponentModel;

namespace Economy.Domain.Enums
{
	public enum TicketStatus
	{
		[Description("Cancelled")]
		Cancelled = 0,
		[Description("Draft")]
		Draft = 1,
		[Description("Confirmed")]
		Confirmed = 2,
		[Description("OnProgress")]
		OnProgress = 3,
		[Description("OnHold")]
		OnHold = 4,
		[Description("Solved")]
		Solved = 5,
		[Description("Archived")]
		Archived = 6
	}
}
