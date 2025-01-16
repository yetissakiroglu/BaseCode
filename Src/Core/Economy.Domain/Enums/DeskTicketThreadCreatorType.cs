using System.ComponentModel;

namespace Economy.Domain.Enums
{
	public enum DeskTicketThreadCreatorType
	{
		[Description("Agent")]
		Agent = 0,
		[Description("Customer")]
		Customer = 1,
		[Description("Vendor")]
		Vendor = 2
	}
}
