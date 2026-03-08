using Application.Interfaces;
using ExternalCommunication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExternalCommunication
{
    public static class ServiceExtensions
    {
        public static void AddExternalServices(this IServiceCollection services)
        {
            services.AddHttpClient<IArgentidaDatosService, ArgentinaDatosService>();
        }
    }
}
