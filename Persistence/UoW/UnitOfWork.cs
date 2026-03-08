using Application.Interfaces;
using Domain.Entities;
using Persistence.Contexts;

namespace Persistence.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IRepositoryAsync<CuotaServicio> CuotaServicioRepository { get; }
        public IRepositoryAsync<Servicio> ServicioRepository { get; }
        public IRepositoryAsync<CotizacionDolar> CotizacionDolarRepository { get; }
        public IRepositoryAsync<TipoDolar> TipoDolarRepository { get; }

        public UnitOfWork (
                ApplicationDbContext context, 
                IRepositoryAsync<CuotaServicio> cuotaServicioRepository, 
                IRepositoryAsync<Servicio> servicioRepository, 
                IRepositoryAsync<CotizacionDolar> cotizacionDolarRepository, 
                IRepositoryAsync<TipoDolar> tipoDolarRepository
            )
        {
            _context = context;
            CuotaServicioRepository = cuotaServicioRepository;
            ServicioRepository = servicioRepository;
            CotizacionDolarRepository = cotizacionDolarRepository;
            TipoDolarRepository = tipoDolarRepository;
        }


        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
