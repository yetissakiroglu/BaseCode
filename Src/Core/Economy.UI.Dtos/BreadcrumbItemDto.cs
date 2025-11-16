using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.UI.Dtos
{
    public sealed record BreadcrumbItemDto(
     string Title,
     string Url,
     bool Active
 );
}
