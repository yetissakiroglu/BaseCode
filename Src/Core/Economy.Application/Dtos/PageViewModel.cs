using Economy.Domain.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Economy.Application.Dtos
{
    public class PageViewModel : PageModel
    {
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageURL { get; set; }
        public string PageImageURL { get; set; }

        public List<string> PageCanonicalURLs { get; set; } = new();
        public List<BreadcrumbDto> Breadcrumb { get; set; } = new ();
    }

}
