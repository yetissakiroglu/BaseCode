using Economy.Domain.Entites.EntityAppDeskTickets;

namespace Economy.Application.Services
{
    public interface IDeskTicketThreadService
	{
		Task<DeskTicketThread?> GetByIdForReadAsync(string? id);
		Task<DeskTicketThread?> GetByIdForEditAsync(string? id);
	}
}
