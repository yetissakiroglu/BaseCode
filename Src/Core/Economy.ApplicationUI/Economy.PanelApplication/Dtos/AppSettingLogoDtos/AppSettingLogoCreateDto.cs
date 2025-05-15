using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppSettingLogoDtos
{
    public class AppSettingLogoCreateDto
    {
        public int Id { get; set; }
        public string LogoPath { get; set; } = string.Empty;
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
    }
}
