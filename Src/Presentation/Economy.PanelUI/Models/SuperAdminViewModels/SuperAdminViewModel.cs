namespace Economy.Panel.UI.Models.SuperAdminViewModels
{
    public class SuperAdminViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDefaultAdmin { get; set; }
        public bool IsDeleted { get; set; }
        public string? PhotoUrl { get; set; }
        public string? JobTitle { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

    }
}
