using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class user_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    PublicationStatus = table.Column<int>(type: "int", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCategories_AppCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRTL = table.Column<bool>(type: "bit", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLogoSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebLogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MobileLogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogoSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppMenus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMenus_AppMenus_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "AppMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHomePage = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SectionType = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSiteLive = table.Column<bool>(type: "bit", nullable: false),
                    ForceSSL = table.Column<bool>(type: "bit", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCDN = table.Column<bool>(type: "bit", nullable: false),
                    StaticFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableDebugMode = table.Column<bool>(type: "bit", nullable: false),
                    CustomCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomJs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCache = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSlides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSectionId = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSlides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppTechnicalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSiteLive = table.Column<bool>(type: "bit", nullable: false),
                    ForceSSL = table.Column<bool>(type: "bit", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EnableCDN = table.Column<bool>(type: "bit", nullable: false),
                    StaticFileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EnableDebugMode = table.Column<bool>(type: "bit", nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EnableCache = table.Column<bool>(type: "bit", nullable: false),
                    CustomCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomJs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableMaintenanceIpWhitelist = table.Column<bool>(type: "bit", nullable: false),
                    AllowedIpAddresses = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnableCustomHeaderScripts = table.Column<bool>(type: "bit", nullable: false),
                    EnableCustomFooterScripts = table.Column<bool>(type: "bit", nullable: false),
                    GoogleAnalyticsCode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FacebookPixelCode = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    EnableGlobalScriptInjection = table.Column<bool>(type: "bit", nullable: false),
                    EnablePreloader = table.Column<bool>(type: "bit", nullable: false),
                    PreloaderHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTechnicalSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDefaultAdmin = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRefreshTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRefreshTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Thumbnail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    PublicationStatus = table.Column<int>(type: "int", nullable: false),
                    AppCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContents_AppCategories_AppCategoryId",
                        column: x => x.AppCategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppCategoryTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppCategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCategoryTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCategoryTranslation_AppCategories_AppCategoryId",
                        column: x => x.AppCategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCategoryTranslation_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppMenuTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppMenuId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMenuTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                        column: x => x.AppMenuId,
                        principalTable: "AppMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPageTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppPageId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageTranslations_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPageTranslations_AppPages_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPageSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    AppPageId = table.Column<int>(type: "int", nullable: false),
                    AppSectionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageSections_AppPages_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPageSections_AppSections_AppSectionId",
                        column: x => x.AppSectionId,
                        principalTable: "AppSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSectionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSectionId = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSectionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSectionImages_AppSections_AppSectionId",
                        column: x => x.AppSectionId,
                        principalTable: "AppSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSettingTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSettingId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    SiteTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslations_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                        column: x => x.AppSettingId,
                        principalTable: "AppSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSlideTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSlideId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    ButtonText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ButtonIcon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSlideTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSlideTranslations_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                        column: x => x.AppSlideId,
                        principalTable: "AppSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppContentTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContentTranslations_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppContentTranslations_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppLanguages",
                columns: new[] { "Id", "Code", "Icon", "IsActive", "IsDefault", "IsDeleted", "IsRTL", "Name" },
                values: new object[,]
                {
                    { 1, "tr", "🇹🇷", true, true, false, false, "Türkçe" },
                    { 2, "en", "🇬🇧", true, false, false, false, "English" },
                    { 3, "ar", "🇸🇦", true, false, false, true, "العربية" }
                });

            migrationBuilder.InsertData(
                table: "AppLogoSettings",
                columns: new[] { "Id", "IsDeleted", "MobileLogoPath", "WebLogoPath" },
                values: new object[] { 1, false, "img/logo_m.png", "img/logo.png" });

            migrationBuilder.InsertData(
                table: "AppMenus",
                columns: new[] { "Id", "IsDeleted", "IsExternal", "ParentMenuId" },
                values: new object[,]
                {
                    { 1, false, false, null },
                    { 2, false, false, null },
                    { 3, false, false, null },
                    { 4, false, false, null },
                    { 5, false, false, null },
                    { 6, false, false, null }
                });

            migrationBuilder.InsertData(
                table: "AppPages",
                columns: new[] { "Id", "IsDeleted", "IsHomePage" },
                values: new object[] { 1, false, true });

            migrationBuilder.InsertData(
                table: "AppSections",
                columns: new[] { "Id", "Content", "IsDeleted", "Name", "SectionType" },
                values: new object[,]
                {
                    { 1, "", false, "Anasayfa Slider", 2 },
                    { 2, "Doğanın nefes kesen güzelliğinin turkuaz sularla buluştuğu Kemer’ in kalbinde konumlanan Türkiz Resort Hotel göz alıcı mimarisi ve sıcak atmosferi ile sizi eşsiz bir mutluluğa davet ediyor.", false, "Imperial Turkiz Resort Hotel", 1 }
                });

            migrationBuilder.InsertData(
                table: "AppSlides",
                columns: new[] { "Id", "AppSectionId", "IsDeleted", "Sequence", "Thumbnail" },
                values: new object[,]
                {
                    { 1, 1, false, 1, "slide_1.jpg" },
                    { 2, 1, false, 1, "slide_2.jpg" },
                    { 3, 1, false, 1, "slide_3.jpg" }
                });

            migrationBuilder.InsertData(
                table: "AppTechnicalSettings",
                columns: new[] { "Id", "AllowedIpAddresses", "AppVersion", "CustomCss", "CustomJs", "DomainName", "EnableCDN", "EnableCache", "EnableCustomFooterScripts", "EnableCustomHeaderScripts", "EnableDebugMode", "EnableGlobalScriptInjection", "EnableMaintenanceIpWhitelist", "EnablePreloader", "FacebookPixelCode", "ForceSSL", "GoogleAnalyticsCode", "IsDeleted", "IsSiteLive", "MaintenanceMessage", "PreloaderHtml", "StaticFileUrl" },
                values: new object[] { 1, null, "v1.0.0", "", "", "www.otelsitem.com", false, true, false, false, false, false, false, false, null, true, null, false, true, "Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.", null, "" });

            migrationBuilder.InsertData(
                table: "AppMenuTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppMenuId", "IsDeleted", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Ana Menü", "ana-menu" },
                    { 2, 2, 1, false, "Main Menu", "main-menu" },
                    { 3, 1, 2, false, "Odalar & Süitler", "odalar-suitler" },
                    { 4, 2, 2, false, "Rooms & Suites", "rooms-suites" },
                    { 5, 1, 3, false, "Restoran & Bar", "restoran-bar" },
                    { 6, 2, 3, false, "Restaurant & Bar", "restaurant-bar" },
                    { 7, 1, 4, false, "Spa & Wellness", "spa-wellness" },
                    { 8, 2, 4, false, "Spa & Wellness", "spa-wellness" },
                    { 9, 1, 5, false, "Hakkımızda", "hakkimizda" },
                    { 10, 2, 5, false, "About Us", "about-us" },
                    { 11, 1, 6, false, "İletişim", "iletisim" },
                    { 12, 2, 6, false, "Contact", "contact" }
                });

            migrationBuilder.InsertData(
                table: "AppPageTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppPageId", "Content", "IsDeleted", "MetaDescription", "MetaTitle", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "Anasayfa", false, "Anasayfa", "Anasayfa", "Anasayfa", "anasayfa" },
                    { 2, 2, 1, "Home", false, "Home", "Home", "Home", "home" }
                });

            migrationBuilder.InsertData(
                table: "AppSectionImages",
                columns: new[] { "Id", "AppSectionId", "Content", "IsDeleted", "Name", "Sequence", "Thumbnail" },
                values: new object[,]
                {
                    { 1, 2, "", false, "Kurumsal 1", 1, "essiz-misafirperverligi-595bb.jpg" },
                    { 2, 2, "", false, "Kurumsal 2", 2, "essiz-osmanli-stili-ve-misafirperverligi-7315a.jpg" }
                });

            migrationBuilder.InsertData(
                table: "AppSlideTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppSlideId", "ButtonIcon", "ButtonText", "ButtonUrl", "Content", "IsDeleted", "IsExternal", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 2, 2, 1, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" },
                    { 3, 1, 2, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 4, 2, 2, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" },
                    { 5, 1, 3, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 6, 2, 3, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCategoryTranslation_AppCategoryId",
                table: "AppCategoryTranslation",
                column: "AppCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCategoryTranslation_AppLanguageId",
                table: "AppCategoryTranslation",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContents_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContentTranslations_AppContentId",
                table: "AppContentTranslations",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContentTranslations_AppLanguageId",
                table: "AppContentTranslations",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMenus_ParentMenuId",
                table: "AppMenus",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMenuTranslations_AppLanguageId",
                table: "AppMenuTranslations",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMenuTranslations_AppMenuId",
                table: "AppMenuTranslations",
                column: "AppMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageSections_AppPageId",
                table: "AppPageSections",
                column: "AppPageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageSections_AppSectionId",
                table: "AppPageSections",
                column: "AppSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageTranslations_AppLanguageId",
                table: "AppPageTranslations",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageTranslations_AppPageId",
                table: "AppPageTranslations",
                column: "AppPageId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AppSectionImages_AppSectionId",
                table: "AppSectionImages",
                column: "AppSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslations_AppLanguageId",
                table: "AppSettingTranslations",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslations_AppSettingId",
                table: "AppSettingTranslations",
                column: "AppSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSlideTranslations_AppLanguageId",
                table: "AppSlideTranslations",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSlideTranslations_AppSlideId",
                table: "AppSlideTranslations",
                column: "AppSlideId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCategoryTranslation");

            migrationBuilder.DropTable(
                name: "AppContentTranslations");

            migrationBuilder.DropTable(
                name: "AppLogoSettings");

            migrationBuilder.DropTable(
                name: "AppMenuTranslations");

            migrationBuilder.DropTable(
                name: "AppPageSections");

            migrationBuilder.DropTable(
                name: "AppPageTranslations");

            migrationBuilder.DropTable(
                name: "AppSectionImages");

            migrationBuilder.DropTable(
                name: "AppSettingTranslations");

            migrationBuilder.DropTable(
                name: "AppSlideTranslations");

            migrationBuilder.DropTable(
                name: "AppTechnicalSettings");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRefreshTokens");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "AppContents");

            migrationBuilder.DropTable(
                name: "AppMenus");

            migrationBuilder.DropTable(
                name: "AppPages");

            migrationBuilder.DropTable(
                name: "AppSections");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "AppLanguages");

            migrationBuilder.DropTable(
                name: "AppSlides");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppCategories");
        }
    }
}
