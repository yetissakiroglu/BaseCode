using Economy.Core.Enums;

namespace Economy.Application.AdminUI.Dtos.AppAccountDtos
{
    public sealed class AppLoginResultDto
    {
        public SignInOutcome Outcome { get; set; }
        public AppUserResultDto? User { get; set; }
    }
}
