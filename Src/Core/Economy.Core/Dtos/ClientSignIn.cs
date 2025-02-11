namespace Economy.Core.Dtos
{
    public class ClientSignIn
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}