namespace Economy.Core.Business
{
	public class TokenOption
	{
		public string Issuer { get; set; } = default!;
		public List<string> Audiences { get; set; } = new();
		public int AccessTokenExpiration { get; set; }
		public int RefreshTokenExpiration { get; set; }
		public string SecurityKey { get; set; } = default!;
	}
}
