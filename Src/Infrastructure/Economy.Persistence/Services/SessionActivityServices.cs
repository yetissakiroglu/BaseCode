using Economy.Application.Repositories;
using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Application.Repositories.UserServiceRepositoriesa;
using Economy.Application.Services;
using Economy.Application.UnitOfWorks;
using Economy.Domain.Entites;
using Economy.Domain.Enums;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Services.BaseServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Economy.Persistence.Services
{

    public class SessionActivityServices(ISessionActivityRepository repository, IAppUserRepository userRepository, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration,
  IHttpContextAccessor httpContextAccessor, IAuditColumnTransformer auditColumnTransformer, AppDbContext appContext)
  : Service<SessionActivity, string>(repository, unitOfWork, dateFormatConfiguration, httpContextAccessor, auditColumnTransformer, appContext), ISessionActivityServices
    {
        private readonly IAppUserRepository _userRepository = userRepository;
        private readonly ISessionActivityRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        protected readonly DateFormatConfiguration _dateFormatConfiguration = dateFormatConfiguration.Value;
        protected readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        protected readonly IAuditColumnTransformer _auditColumnTransformer = auditColumnTransformer;

        public async Task RecordActivityLoginAsync(string email)
        {
            var user = await _userRepository.Table.Where(x => !(string.IsNullOrEmpty(x.UserName)) && x.UserName.Equals(email)).FirstOrDefaultAsync();
            if (user != null)
            {
                var entity = new SessionActivity
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = user.UserName,
                    UserId = user.Id,
                    ActivityDateTimeUtc = DateTime.UtcNow,
                    ActivityType = SessionActivityType.Login,
                    ActivityTypeString = SessionActivityType.Login.ToString()
                };
                await AddAsync(entity);

                user.IsOnline = true;
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task RecordActivityLogoutAsync()
        {
            var email = "";
            var user = await _userRepository.Table.Where(x => !(string.IsNullOrEmpty(x.UserName)) && x.UserName.Equals(email)).FirstOrDefaultAsync();
            if (user != null)
            {
                var entity = new SessionActivity
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    ActivityDateTimeUtc = DateTime.UtcNow,
                    ActivityType = SessionActivityType.Logout,
                    ActivityTypeString = SessionActivityType.Logout.ToString()
                };
                await AddAsync(entity);

                user.IsOnline = false;
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
