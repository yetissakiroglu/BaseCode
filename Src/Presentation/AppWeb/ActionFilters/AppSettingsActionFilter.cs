using Economy.Application.Interfaces;
using Economy.Application.Queries.AppSettings;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AppWeb.ActionFilters
{
    public class AppSettingsActionFilter : IAsyncActionFilter
    {
        private readonly IAppSettingService _appSettingsService;

        public AppSettingsActionFilter(IAppSettingService appSettingsService)
        {
            _appSettingsService = appSettingsService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // AppSettings verilerini alın
            var appSettings = await _appSettingsService.GetForReadAsync(new GetAppSettingQuery());

            // ViewData'ya veya ViewBag'a atama yaparak view'da kullanılabilir hale getirin
            context.HttpContext.Items["AppSettings"] = appSettings.Data;

            // Bir sonraki filtreyi çalıştırın (action'ı çağırın)
            await next();
        }
    }
}
