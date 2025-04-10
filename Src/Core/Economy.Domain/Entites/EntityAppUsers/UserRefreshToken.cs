using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppUsers
{
	public class UserRefreshToken : BaseEntity<string>
	{
		public string UserId { get; set; } = default!;
		public string Token { get; set; } = default!;
		public DateTime Expiration { get; set; }
	}
}
