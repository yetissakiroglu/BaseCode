using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntitySlides;
using Economy.Panel.Application.Dtos.AppSettingDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSlideService : IPanelAppSlideService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSlide, int> _entityRepository;

        public PanelAppSlideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppSlide>();
        }


        public ResponseModel<AppSlideDto> CreateSlide(AppSlideCreateDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSlideDto> DeleteSlide(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSlideDto> EditSlide(AppSlideEditDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<IEnumerable<AppSlideDto>> GetAllSlide(bool isDeleted)
        {
            var result = _entityRepository.WhereForReadFunc(w => w.IsDeleted == isDeleted,
                                x => x.Include(y => y.Translations));

           var response = result.ToList();
            var resultModel = new AppSlideDto[response.Count];

            return ResponseModel<IEnumerable<AppSlideDto>>.Success(resultModel, HttpStatusCode.OK);
        }

        public ResponseModel<AppSlideDto> GetSlide(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }
    }
}
