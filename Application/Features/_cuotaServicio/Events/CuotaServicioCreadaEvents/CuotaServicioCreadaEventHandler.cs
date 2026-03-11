using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features._cuotaServicio.Events.CuotaServicioCreadaEvents
{
    public class CuotaServicioCreadaEventHandler : INotificationHandler<CuotaServicioCreadaEvent>
    {
        private readonly ILogger<CuotaServicioCreadaEventHandler> _logger;

        public CuotaServicioCreadaEventHandler(ILogger<CuotaServicioCreadaEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(CuotaServicioCreadaEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event Recibido: Cuota de servicio con ID {CuotaServicioId} ha sido creada exitosamente.", notification.CuotaServicioId);
            
            // Aquí a futuro se pueden inyectar dependencias y ejecutar lógica adicional (ej: enviar mail)
            
            return Task.CompletedTask;
        }
    }
}
