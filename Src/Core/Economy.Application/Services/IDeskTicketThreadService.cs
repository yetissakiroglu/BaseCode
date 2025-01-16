using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityAppDeskTickets;

namespace Economy.Application.Services
{
	public interface IDeskTicketThreadService : IService<DeskTicketThread,int>
	{
		Task<DeskTicketThread?> GetByIdForReadAsync(string? id);
		Task<DeskTicketThread?> GetByIdForEditAsync(string? id);
	}
}
