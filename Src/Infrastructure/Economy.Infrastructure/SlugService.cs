using Economy.Application.Interfaces;
using Economy.Core.Dtos;
using Economy.Core.Extensions;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Economy.Infrastructure
{
    public class SlugService : ISlugService
    {
        private readonly IEntityRepository<AppPageTranslation, int> _appPageTranslationRepository;
        private readonly SeoOptions _opt;
        private readonly IUnitOfWork _unitOfWork;
        public SlugService(IOptions<SeoOptions> opt, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appPageTranslationRepository = unitOfWork.HotelEntityRepository<AppPageTranslation>();
            _opt = opt.Value;
        }

        public async Task<string> GenerateForPageTranslationAsync(
               string title,
               int languageId,
               int? pageTranslationId = null,
               CancellationToken ct = default)
        {
            // 1) Başlıktan baz slug üret
            var baseSlug = SlugHelper.ToSlug(title, _opt.SlugMode, _opt.MaxLength);

            if (!_opt.EnsureUnique)
                return baseSlug;

            // 2) Dil bazlı benzersizlik (PageTranslation üzerinde)
            string slug = baseSlug;
            int i = 2;

            while (await _appPageTranslationRepository.DataSet.AsNoTracking()
                .AnyAsync(t =>
                    t.AppLanguageId == languageId &&
                    t.Slug == slug &&
                    (pageTranslationId == null || t.Id != pageTranslationId),
                    ct))
            {
                slug = $"{baseSlug}-{i++}";
            }

            return slug;
        }
    }
}
