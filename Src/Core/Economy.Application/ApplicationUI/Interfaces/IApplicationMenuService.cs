using Economy.Application.ApplicationUI.Dtos;

namespace Economy.Application.ApplicationUI.Interfaces
{
  

    public interface IApplicationMenuService
    {
        Task<List<MenuNodeDto>> GetMenuAsync(string lang, CancellationToken ct);


    }
}
