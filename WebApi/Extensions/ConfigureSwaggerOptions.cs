using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Extensions
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo
                {
                    Title = "API_Dolar",
                    Description = "Sistema de Gestión de Pagos y Cotización de Dólar",
                    Version = desc.ApiVersion.ToString(),
                    Contact = new OpenApiContact
                    {
                        Name = "Lucas Ludu",
                        Email = "lucas@gmail.com",
                        Url = new Uri("https://github.com/LucasLudu")
                    }
                });
            }
        }
    }
}
