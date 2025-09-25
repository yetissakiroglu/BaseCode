using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryReservationService : IReservationService
    {
        public Task<(bool ok, string? reference)> CreateAsync(ReservationRequestDto dto)
        {
            if (dto.CheckOut <= dto.CheckIn) return Task.FromResult((false, (string?)null));
            return Task.FromResult((true, $"RSV-{DateTime.UtcNow:yyyyMMddHHmmss}"));
        }
    }

}
