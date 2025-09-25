namespace Economy.Web.UI.Services.Abstractions
{
    public record ReservationRequestDto(string Name, string Email, string Phone, DateOnly CheckIn, DateOnly CheckOut, string RoomSlug, int Guests, string? Notes);
    public interface IReservationService
    {
        Task<(bool ok, string? reference)> CreateAsync(ReservationRequestDto dto);
    }

}
