using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Providers
{
    public sealed class TenantContext
    {
        public int? AppId { get; set; }               // aktif otel (App.Id)
        public string? ConnectionString { get; set; } // o App’in DB bağlantısı
    }

    public interface ITenantContextAccessor
    {
        TenantContext Current { get; }
    }
}
