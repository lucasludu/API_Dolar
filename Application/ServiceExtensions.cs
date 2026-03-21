using Application.Behaviours;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ServiceExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly())); 
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddMemoryCache();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            
            services.AddScoped<ICotizacionDolarManager, CotizacionDolarManager>();
        }
    }
}
