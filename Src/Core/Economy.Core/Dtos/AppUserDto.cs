namespace Economy.Core.Dtos
{
    public class AppUserDto
    {
        public int Id { get; set; } // IdentityUser<int>
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}