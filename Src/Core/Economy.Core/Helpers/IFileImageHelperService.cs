using Economy.Core.Helpers.Dtos;
using Economy.Core.Tools;
using Microsoft.AspNetCore.Http;

namespace Economy.Core.Helpers
{
    public interface IFileImageHelperService
    {

        Task<ResponseModel<UploadFile>> UploadFile(IFormFile file, List<string> folderPaths);
        Task<ResponseModel<List<UploadFile>>> UploadFiles(List<IFormFile> files, List<string> folderPaths);
    }
}
