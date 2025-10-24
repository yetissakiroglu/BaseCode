using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Dtos
{
    public record SaveGroupLayoutRequest(int GroupId, List<GroupLayoutItemDto> Items);

}
