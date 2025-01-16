using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites
{
	public class SessionActivity : BaseEntity<string>
    {
        public SessionActivity() { }

        public SessionActivityType? ActivityType { get; set; } = SessionActivityType.Login;
        public string? ActivityTypeString { get; set; } = string.Empty;
        public DateTime? ActivityDateTimeUtc { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;

    }
}
