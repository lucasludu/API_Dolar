using MediatR;

namespace Application.Features._cuotaServicio.Events.CuotaServicioCreadaEvents
{
    public record CuotaServicioCreadaEvent(int CuotaServicioId) : INotification;
  
}
