using Economy.Application.Services.AppContentServices;
using Economy.Application.Services.AppMenuServices;
using Economy.Application.Services.AppSectionServices;
using Microsoft.AspNetCore.Mvc;

namespace Economy.AuthServer.API.Controllers
{

    public class AppController(IAppSectionService appSectionService ,IAppPageService appPageService, IAppMenuService appMenuService, IAppContentService appContentService,IAppCategoryService appCategoryService) : BaseController
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        private readonly IAppContentService _appContentService = appContentService;
        private readonly IAppCategoryService _appCategoryService = appCategoryService;
        private readonly IAppPageService _appPageService = appPageService;
        private readonly IAppSectionService _appSectionService = appSectionService;
        [HttpGet]
        public async Task<IActionResult> Menus()
        {
            var dataModel = await _appMenuService.AppMenusAsync();
            return CreateResult(dataModel);
        }

        [HttpGet]
        public async Task<IActionResult> Headlines(int rowCount)
        {
            var resultModel = await _appContentService.AppContentHeadlineListAsync(rowCount);
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> Featureds(int rowCount)
        {
            var resultModel = await _appContentService.AppContentFeaturedListAsync(rowCount);
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> BreakingNews(int rowCount)
        {
            var resultModel = await _appContentService.AppContentBreakingNewsListAsync(rowCount);
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> ContentDetail(int appContentId)
        {
            var resultModel = await _appContentService.AppContentDetailAsync(appContentId);
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> CategoryDetail(string url)
        {
            var resultModel = await _appCategoryService.AppCategoryDetailAsync(url);
            return CreateResult(resultModel);
        }
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var resultModel = await _appCategoryService.AppCategoriesAsync();
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> PageDetail(string url)
        {
            var resultModel = await _appPageService.AppPageDetailAsync(url);
            return CreateResult(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> Section(int sectionId)
        {
            var resultModel = await _appSectionService.AppSectionAsync(sectionId);
            return CreateResult(resultModel);
        }


        [HttpGet]
        public async Task<IActionResult> PageDetailDefault()
        {
            var resultModel = await _appPageService.AppPageDetailDefaultAsync();
            return CreateResult(resultModel);
        }



    }
}
