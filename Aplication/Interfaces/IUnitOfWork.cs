using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<CuotaServicio> CuotaServicioRepository { get; }
        IRepositoryAsync<CotizacionDolar> CotizacionDolarRepository { get; }
        IRepositoryAsync<Servicio> ServicioRepository { get; }
        IRepositoryAsync<TipoDolar> TipoDolarRepository { get; }
        Task<int> CommitAsync();
    }
}
