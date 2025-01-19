using Core.Models.Business;
using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos;
using Economy.Application.Infrastructure.ConfigurationModels;
using Economy.Application.Infrastructure.Services;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Repositories;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Application.Repositories.AppSectionRepositories;
using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Application.Repositories.UserServiceRepositoriesa;
using Economy.Application.Services;
using Economy.Application.Services.AppContentServices;
using Economy.Application.Services.AppSectionServices;
using Economy.Application.Services.AppUserServices;
using Economy.Application.Services.FileServices;
using Economy.Application.UnitOfWorks;
using Economy.Board.Infrastructure.ConfigurationModels;
using Economy.Core.Business;
using Economy.Core.Services;
using Economy.Domain.Entites.Identities;
using Economy.Infrastructure.Countries;
using Economy.Infrastructure.Currencies;
using Economy.Infrastructure.DateFormats;
using Economy.Infrastructure.Emails;
using Economy.Infrastructure.Providers;
using Economy.Infrastructure.Services;
using Economy.Infrastructure.Services.FileHelperServices;
using Economy.Infrastructure.TimeZones;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Economy.Persistence.Repositories.AppContentRepositories;
using Economy.Persistence.Repositories.AppMenuRepositories;
using Economy.Persistence.Repositories.AppSectionRepositories;
using Economy.Persistence.Repositories.AppSettingRepositories;
using Economy.Persistence.Repositories.UserServiceRepositories;
using Economy.Persistence.Services;
using Economy.Persistence.Services.AppContentServices;
using Economy.Persistence.Services.AppFileServices;
using Economy.Persistence.Services.AppSectionServices;
using Economy.Persistence.Services.AppUserServices;
using Economy.Persistence.UnitOfWorks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), configure =>
	{
		configure.MigrationsAssembly("Economy.Persistence");
	});
});

builder.Services.AddScoped<ILanguageProvider, LanguageProvider>();
builder.Services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IFileImageHelperService, FileImageHelperService>();

builder.Services.AddScoped(typeof(IEntityRepository<,>), typeof(EfEntityRepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IAppSettingRepository, AppSettingRepository>();
builder.Services.AddScoped<IAppSettingServices, AppSettingServices>();

builder.Services.AddScoped<IAppCategoryRepository, AppCategoryRepository>();
builder.Services.AddScoped<IAppCategoryService, AppCategoryService>();

builder.Services.AddScoped<IAppContentRepository, AppContentRepository>();
builder.Services.AddScoped<IAppContentService, AppContentService>();

builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
builder.Services.AddScoped<IAppUserServices, AppUserServices>();

builder.Services.AddScoped<ISessionActivityRepository, SessionActivityRepository>();
builder.Services.AddScoped<ISessionActivityServices, SessionActivityServices>();

//Doküman Resim,Dosya
builder.Services.AddScoped<IFileImageService, FileImageService>();
builder.Services.AddScoped<IFileDocumentService, FileDocumentService>();

builder.Services.AddScoped<IAppContent_DocumentRepository, AppContent_DocumentRepository>();
builder.Services.AddScoped<IAppContent_DocumentService, AppContent_DocumentService>();

builder.Services.AddScoped<IAppTagRepository, AppTagRepository>();
builder.Services.AddScoped<IAppTagService, AppTagService>();

builder.Services.AddScoped<IAppCommentRepository, AppCommentRepository>();
builder.Services.AddScoped<IAppCommentService, AppCommentService>();

builder.Services.AddScoped<IAppMenuRepository, AppMenuRepository>();
builder.Services.AddScoped<IAppMenuService, AppMenuService>();

builder.Services.AddScoped<IAppSectionRepository, AppSectionRepository>();
builder.Services.AddScoped<IAppSectionService, AppSectionService>();

builder.Services.AddScoped<IAppPageRepository, AppPageRepository>();
builder.Services.AddScoped<IAppPageService, AppPageService>();



//builder.Services.AddScoped<IDeskTicketRepository, DeskTicketRepository>();
//builder.Services.AddScoped<IDeskTicketService, DeskTicketServices>();

//builder.Services.AddScoped<IDeskTicketThreadRepository, DeskTicketThreadRepository>();
//builder.Services.AddScoped<IDeskTicketThreadService, DeskTicketThreadService>();

//builder.Services.AddScoped<IEmailSender, SMTPEmailService>();

builder.Services.AddScoped<ITimeZoneService, TimeZoneService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddScoped<IAppSettingServices, AppSettingServices>();
builder.Services.AddScoped<IRequestHandler<CreateAppMenuCommand, int>, CreateAppMenuCommandHandler>();
builder.Services.AddScoped<IRequestHandler<UpdateAppMenuCommand, bool>, UpdateAppMenuCommandHandler>();
builder.Services.AddScoped<IRequestHandler<DeleteAppMenuCommand, bool>, DeleteAppMenuCommandHandler>();
builder.Services.AddScoped<IRequestHandler<GetAppMenuQuery, AppMenuDto>, GetAppMenuQueryHandler>();
builder.Services.AddScoped<IRequestHandler<GetAllAppMenusQuery, List<AppMenuDto>>, GetAllAppMenusQueryHandler>();

// Add services to the container.
builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));



builder.Services.
Configure<FileUploadConfiguration>(builder.Configuration.GetSection("FileUploadConfiguration"));

builder.Services.
	Configure<AppSettingConfiguration>(builder.Configuration.GetSection("AppSettings"));

builder.Services.
	Configure<PagingConfiguration>(builder.Configuration.GetSection("PagingConfiguration"));

builder.Services.
	Configure<SMTPConfiguration>(builder.Configuration.GetSection("SmtpConfiguration"));

builder.Services.
	Configure<RegistrationConfiguration>(builder.Configuration.GetSection("RegistrationConfiguration"));

builder.Services.
	Configure<DateFormatConfiguration>(builder.Configuration.GetSection("DateFormatConfiguration"));

//Kullama Ayarlarý Kesin Girilmesi lazým
builder.Services.
	Configure<ApplicationConfiguration>(builder.Configuration.GetSection("ApplicationConfiguration"));

builder.Services
	.Configure<IdentitySettings>(builder.Configuration.GetSection(IdentitySettings.IdentitySettingsName));

//builder.Services.AddAuthentication(builder.Configuration.GetSection("TokenOption").Get<TokenOption>());

var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = tokenOptions.Issuer,
		ValidAudience = tokenOptions.Audiences.FirstOrDefault(),
		IssuerSigningKey = SignInService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
		ValidateIssuerSigningKey = true,
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero
	};

	options.Events = new JwtBearerEvents
	{
		OnTokenValidated = ctx => Task.CompletedTask,
		OnAuthenticationFailed = ctx => Task.CompletedTask
	};
});

builder.Services
	.AddIdentity<AppUser, IdentityRole>(options =>
	{
		var identitySettings = builder.Configuration.GetSection(IdentitySettings.IdentitySettingsName).Get<IdentitySettings>();
		if (identitySettings != null)
		{
			options.SignIn.RequireConfirmedAccount = identitySettings.RequireConfirmedAccount;
			options.Password.RequireDigit = identitySettings.RequireDigit;
			options.Password.RequiredLength = identitySettings.RequiredLength;
			options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
			options.Password.RequireUppercase = identitySettings.RequireUppercase;
			options.Password.RequireLowercase = identitySettings.RequireLowercase;
			options.Lockout.DefaultLockoutTimeSpan = identitySettings.DefaultLockoutTimeSpan;
			options.Lockout.MaxFailedAccessAttempts = identitySettings.MaxFailedAccessAttempts;
		}
	})
	.AddEntityFrameworkStores<AppDbContext>()
	.AddRoles<IdentityRole>()
	.AddDefaultTokenProviders();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
