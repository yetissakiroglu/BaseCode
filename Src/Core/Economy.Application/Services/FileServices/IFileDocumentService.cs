using Economy.Domain.Entites.EntityAppFiles;
using Microsoft.AspNetCore.Http;

namespace Economy.Application.Services.FileServices
{
    public interface IFileDocumentService
    {
        Task<Guid> UploadDocumentAsync(IFormFile? file);
        Task<Guid> UpdateDocumentAsync(Guid? documentId, IFormFile? newFile);
        Task<AppFileDocument> GetDocumentAsync(Guid? id);
    }
}
