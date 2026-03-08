using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        public ApplicationDbContext (
            DbContextOptions<ApplicationDbContext> options,
            IDateTimeService datetime) 
        : base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this._dateTime = datetime;
        }

        public DbSet<CotizacionDolar> CotizacionDolars { get; set; }
        public DbSet<CuotaServicio> CuotaServicios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<TipoDolar> TipoDolars { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
