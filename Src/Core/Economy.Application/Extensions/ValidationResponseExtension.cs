using Economy.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Economy.Application.Extensions
{
	public static class ValidationResponseExtension
	{
		public static void UseValidationResponse(this IServiceCollection services)
		{
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var errors = context.ModelState.Values.Where(w => w.Errors.Count > 0).SelectMany(s => s.Errors).Select(s => s.ErrorMessage);

					var response = ServiceResult<string>.Fail((errors.ToList()), HttpStatusCode.NotFound);

					return new BadRequestObjectResult(response);
				};
			});
		}
	}
}
