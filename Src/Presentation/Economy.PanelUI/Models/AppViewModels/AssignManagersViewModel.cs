namespace Economy.Panel.UI.Models.AppViewModels
{
    public class AssignManagersViewModel
    {
        public int AppId { get; set; }
        public string AppName { get; set; }

        public List<ManagerItem> AllManagers { get; set; }
        public List<int> SelectedManagerIds { get; set; } = new();
    }
    public class ManagerItem
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }
}
