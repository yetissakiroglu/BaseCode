using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.AdminEntity.EntityAppUsers;

namespace Economy.Domain.Entites.AdminEntity.EntityApp
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
