using Economy.Base.Application.Dtos.BaseModels;

namespace Economy.Application.Dtos.AppAccountDtos
{
    public enum SignInOutcome
    {
        Succeeded = 0,
        RequiresTwoFactor = 1,
        LockedOut = 2,
        InvalidCredentials = 3
    }

    public sealed class LoginResultDto
    {
        public SignInOutcome Outcome { get; set; }
        public AppUserDto? User { get; set; }
    }
}
