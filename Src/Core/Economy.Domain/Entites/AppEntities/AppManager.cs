using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.Identities;

namespace Economy.Domain.Entites.AppEntities
{
    public class AppManager : BaseEntity<int>
    {
        public int AppId { get; set; }
        public int UserId { get; set; }

        // Opsiyonel navigation
        public App? App { get; set; }
        public AppUser? User { get; set; }

    }
}
