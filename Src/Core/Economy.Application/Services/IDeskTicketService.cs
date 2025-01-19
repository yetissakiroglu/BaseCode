using Economy.Domain.Entites.EntityAppDeskTickets;

namespace Economy.Application.Services
{
    public interface IDeskTicketService 
	{
		Task<DeskTicket?> GetByIdForReadAsync(string? id);
		Task<DeskTicket?> GetByIdForEditAsync(string? id);

	}
}
