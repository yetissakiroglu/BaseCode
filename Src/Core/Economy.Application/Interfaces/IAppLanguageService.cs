using Economy.Application.Dtos.AppLanguageDtos;
using Economy.Application.Dtos.AppSettingDtos;
using Economy.Application.Queries.AppLanguages;
using Economy.Application.Queries.AppSettings;
using Economy.Core.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Interfaces
{
    public interface IAppLanguageService
    {

        Task<ResponseModel<AppLanguageDto>> GetDefaultForReadAsync(GetAppLanguageByDefaultQuery query);
        Task<ResponseModel<List<AppLanguageDto>>> GetAllForReadAsync(GetAllAppLanguageQuery query);


    }
}
