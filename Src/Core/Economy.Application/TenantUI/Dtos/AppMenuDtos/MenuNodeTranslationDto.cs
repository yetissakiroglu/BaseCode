using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos.AppMenuDtos
{
    public record MenuNodeTranslationDto(int AppLanguageId, string Title, string? Url);

}
