namespace Economy.Core.Enums
{
    public enum SignInOutcome
    {
        Succeeded = 0,
        RequiresTwoFactor = 1,
        LockedOut = 2,
        InvalidCredentials = 3
    }
}
