using Economy.Domain.Entites.EntityAppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppSettingDtos
{
    public class AppSettingDto
    {
        public int Id { get; set; }
        public List<AppSettingTranslationDto> Translations { get; set; }
    }
}
