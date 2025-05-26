using Economy.Core.Helpers.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Http;

namespace Economy.Core.Helpers
{
    public interface IFileImageHelperService
    {
        ServiceResult<UploadFile> UploadBase64(string? base64String, List<string> folderPaths);

        //Task<ResponseModel<UploadFile>> UploadFile(IFormFile file, List<string> folderPaths);
        //Task<ResponseModel<List<UploadFile>>> UploadFiles(List<IFormFile> files, List<string> folderPaths);
    }
}
