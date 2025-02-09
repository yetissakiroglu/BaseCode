using Economy.Domain.Entites;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Domain.Entites.EntityAppFiles;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppPages;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntityAppUsers;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Domain.Entites.EntityPages;
using Economy.Domain.Entites.EntitySlides;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.Configurations.ConfigurationAppContents;
using Economy.Persistence.Configurations.ConfigurationMenuItems;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Economy.Persistence.Contexts
{

    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
	{

        /* Ayarlar */
        public DbSet<AppSetting> AppSettings { get; set; }
        /* Post */
        public DbSet<AppContent> AppContents { get; set; }
        public DbSet<AppImageGroup> AppImageGroups { get; set; }
        public DbSet<AppImage> AppImages { get; set; }
        public DbSet<AppAmenityGroup> AppAmenityGroups { get; set; }
        public DbSet<AppAmenity> AppAmenities { get; set; }
        public DbSet<AppCategory> AppCategories { get; set; }
        public DbSet<AppComment> AppComments { get; set; }
        public DbSet<AppContentTag> AppContentTags { get; set; }
        public DbSet<AppTag> AppTags { get; set; }
        public DbSet<AppContent_Document> AppContent_Documents { get; set; } = default!;
        public DbSet<AppContent_ImportantNote> AppContent_ImportantNotes { get; set; } = default!;
        public DbSet<AppFileImage> AppFileImages { get; set; } = default!;
        public DbSet<AppFileDocument> AppFileDocuments { get; set; } = default!;
        public DbSet<AppMenu> AppMenus { get; set; }



        /*---------------- End -----------------*/
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
		public DbSet<NumberSequence> NumberSequences { get; set; } = default!;
		public DbSet<SessionActivity> SessionActivity { get; set; } = default!;
		public DbSet<AppSlide> AppSlides { get; set; }
		public DbSet<AppSlideTranslation> AppSlideTranslations { get; set; }
		public DbSet<AppLanguage> AppLanguages { get; set; }
		public DbSet<AppPage> AppPages { get; set; }
		public DbSet<AppPageSection> AppPageSections { get; set; }
		public DbSet<AppSection> AppSections { get; set; }
		//--------------------------------------------

		

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
            // AppCategory yapılandırmasını uygulayın
            builder.ApplyConfiguration(new AppContent_Configuration());
            builder.ApplyConfiguration(new AppCategory_Configuration());
            builder.ApplyConfiguration(new AppMenu_Configuration());
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			
			base.OnModelCreating(builder);
		}

	}
}

