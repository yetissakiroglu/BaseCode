namespace Economy.Board.Infrastructure.ConfigurationModels
{
    public record ApplicationConfiguration
    {
        public bool IsDemoVersion { get; init; } = false;
        public bool IsDevelopment { get; init; } = true;
        public string Homepage { get; init; } = string.Empty;
        public string LoginPage { get; init; } = string.Empty;
        public string LogoutPage { get; init; } = string.Empty;
        public string AccessDeniedPage { get; init; } = string.Empty;
        public string AccountProfilePage { get; init; } = string.Empty;
        public string InternalUserRegistrationPage { get; init; } = string.Empty;
        public string EditorUserRegistrationPage { get; init; } = string.Empty;
        public string DefaultPassword { get; init; } = string.Empty;
        public bool TwoFactorEnabled { get; init; } = true;
        public bool ExternalLoginEnabled { get; init; } = true;
    }
}
