using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Economy.Application.Dtos
{
    public class PageViewModel : PageModel
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageURL { get; set; }
        public string PageImageURL { get; set; }

        public List<string> PageCanonicalURLs { get; set; } = [];
        public List<BreadcrumbsViewModel> Breadcrumbs { get; set; } = [];

    }

    public record BreadcrumbsViewModel(string Name, string Url, bool Active);
}
