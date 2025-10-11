using Economy.Domain.BaseEntities;
using Microsoft.AspNetCore.Identity;

namespace Economy.Domain.Entites.AdminEntity.EntityAppUsers
{
    public class AppUserToken : IdentityUserToken<int>, ISoftDelete, IHasId<int>
    {
        public string Token { get; set; } // JWT Access Token
        public string RefreshToken { get; set; } // Refresh Token
        public DateTime ExpirationDate { get; set; } // Refresh Token'ın geçerlilik tarihi
        public bool IsDeleted { get; set; }
        public int Id { get; set; }
    }
}
