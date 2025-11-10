using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Economy.Panel.UI.Tenancy
{
    public sealed class AddTenantHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            // Varsa tekrar ekleme
            if (operation.Parameters.Any(p => p.Name == "X-TENANT")) return;

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-TENANT",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Tenant anahtarı: domain (örnek: xotel.local) veya AppId (örnek: 42)",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Example = new OpenApiString("xotel.local")
                }
            });
        }
    }
}
