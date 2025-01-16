using System.ComponentModel;

namespace Economy.Domain.Enums
{
	public enum TicketPriority
	{
		[Description("Low")]
		Low = 0,
		[Description("Medium")]
		Medium = 1,
		[Description("High")]
		High = 2
	}
}
