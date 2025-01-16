using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppDeskTickets;

namespace Economy.Application.Services
{
	public interface IDeskTicketService : IService<DeskTicket,int>
	{
		Task<DeskTicket?> GetByIdForReadAsync(string? id);
		Task<DeskTicket?> GetByIdForEditAsync(string? id);

	}
}
