using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Domain.Entites.EntitySlides;
using Economy.Panel.Application.Dtos.AppContentDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Interfaces;
using FluentValidation;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppContentService : IPanelAppContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppContent, int> _entityRepository;
        private readonly IFileImageHelperService _fileImageHelperService;
        private readonly IValidator<AppContentCreateEditDto> _validator;
        public PanelAppContentService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<AppContentCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppContent>();
            _fileImageHelperService = fileImageHelperService;
            _validator = validator;
        }

        public ServiceResult<AppContentDto> CreateContent(AppContentCreateEditDto model)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<AppContentDto> DeleteContent(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<AppContentDto> EditContent(AppContentCreateEditDto model)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<List<AppContentDto>> GetAllContent(bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public ServiceResult<AppContentDto> GetContent(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }
    }
}
