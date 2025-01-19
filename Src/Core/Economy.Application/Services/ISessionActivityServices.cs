namespace Economy.Application.Services
{
    public interface ISessionActivityServices 
    {
        Task RecordActivityLoginAsync(string email);
        Task RecordActivityLogoutAsync();
    }
}
