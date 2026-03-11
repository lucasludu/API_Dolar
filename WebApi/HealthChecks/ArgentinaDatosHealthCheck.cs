using Application.Interfaces;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi.HealthChecks
{
    public class ArgentinaDatosHealthCheck : IHealthCheck
    {
        private readonly IArgentidaDatosService _argentinaDatosService;

        public ArgentinaDatosHealthCheck(IArgentidaDatosService argentinaDatosService)
        {
            _argentinaDatosService = argentinaDatosService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var estado = await _argentinaDatosService.GetEstadoApiAsync();
                
                if (estado != null)
                {
                    return HealthCheckResult.Healthy("API de ArgentinaDatos está en funcionamiento.");
                }

                return HealthCheckResult.Unhealthy("No se pudo obtener el estado de la API de ArgentinaDatos.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Error al contactar la API de ArgentinaDatos: {ex.Message}");
            }
        }
    }
}
