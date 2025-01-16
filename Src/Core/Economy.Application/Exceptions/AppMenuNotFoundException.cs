namespace Economy.Application.Exceptions
{
    public class AppMenuNotFoundException : Exception
    {
        public AppMenuNotFoundException(int id)
            : base($"AppMenu with ID {id} was not found.") { }
    }
}
