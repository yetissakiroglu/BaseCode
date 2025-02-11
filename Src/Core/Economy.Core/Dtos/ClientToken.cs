using System;

namespace Economy.Core.Dtos
{
    public class ClientToken
    {
        public string AccessToken { get; set; } = default!;
        public DateTime Expiration { get; set; }
    }
}