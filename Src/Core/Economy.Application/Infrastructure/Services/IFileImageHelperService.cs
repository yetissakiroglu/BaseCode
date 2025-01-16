using Economy.Infrastructure.Services.FileHelperServices.Models;
using Economy.Persistence.Services;
using Microsoft.AspNetCore.Http;

namespace Economy.Application.Infrastructure.Services
{
    public interface IFileImageHelperService
    {

        Task<ServiceResult<UploadFile>> UploadFile(IFormFile file, List<string> folderPaths);
        Task<ServiceResult<List<UploadFile>>> UploadFiles(List<IFormFile> files, List<string> folderPaths);
    }
}
