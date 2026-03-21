using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._cuotaServicio.Response;
using Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ardalis.Specification;
using MediatR;

namespace Application.UnitTests.Features._cuotaServicio.Command.CargarCuotasServicioCommands
{
    public class CuotaServicioCommandHandlerTests
    {
        private readonly Mock<IRepositoryAsync<CuotaServicio>> _cuotaRepositoryMock;
        private readonly Mock<IRepositoryAsync<Servicio>> _servicioRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICotizacionDolarManager> _cotizacionDolarManagerMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CargarCuotasServicioCommand>> _loggerMock;
        private readonly Mock<IArgentidaDatosService> _argentinaDatosServiceMock;
        private readonly CargarCuotasServicioCommandHandler _handler;

        public CuotaServicioCommandHandlerTests()
        {
            _cuotaRepositoryMock = new Mock<IRepositoryAsync<CuotaServicio>>();
            _servicioRepositoryMock = new Mock<IRepositoryAsync<Servicio>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cotizacionDolarManagerMock = new Mock<ICotizacionDolarManager>();
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CargarCuotasServicioCommand>>();
            _argentinaDatosServiceMock = new Mock<IArgentidaDatosService>();

            _unitOfWorkMock.Setup(u => u.RepositoryAsync<CuotaServicio>()).Returns(_cuotaRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.RepositoryAsync<Servicio>()).Returns(_servicioRepositoryMock.Object);

            _handler = new CargarCuotasServicioCommandHandler(
                _unitOfWorkMock.Object,
                _cotizacionDolarManagerMock.Object,
                _mediatorMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _argentinaDatosServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Calculate_USD_When_Base_Is_ARS()
        {
            // Arrange
            var request = new CuotaServicioRequest
            {
                NumeroCuota = 1,
                FechaPago = DateTime.Now,
                MontoARS = 1000,
                MontoUSD = 0,
                ServicioId = 1,
                TipoDolar = "oficial",
                DeterminaCuotaPor = "ARS"
            };
            var command = new CargarCuotasServicioCommand(request);
            var cotizacion = new CotizacionDolar(DateTime.Now, "oficial", 900, 1000);
            cotizacion.Id = 1;

            var servicio = new Servicio("Fiat", DateTime.Now, null, true);
            servicio.Id = 1;

            _cuotaRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<CuotaServicio>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CuotaServicio?)null);
            _servicioRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(servicio);
            //_cotizacionDolarManagerMock.Setup(c => c.ObtenerYGuardarCotizacionAsync("oficial", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(cotizacion);
            _cuotaRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CuotaServicio>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuotaServicio(1, DateTime.Now, 1000, 1, 1, 1));
            _mapperMock.Setup(x => x.Map<CuotaServicioResponse>(It.IsAny<CuotaServicio>()))
                .Returns(new CuotaServicioResponse { Id = 1 });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _cuotaRepositoryMock.Verify(x => x.AddAsync(It.Is<CuotaServicio>(c => c.MontoUSD == 1), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Calculate_ARS_When_Base_Is_USD()
        {
            // Arrange
            var request = new CuotaServicioRequest
            {
                NumeroCuota = 1,
                FechaPago = DateTime.Now,
                MontoARS = 0,
                MontoUSD = 2,
                ServicioId = 1,
                TipoDolar = "oficial",
                DeterminaCuotaPor = "USD"
            };
            var command = new CargarCuotasServicioCommand(request);
            var cotizacion = new CotizacionDolar(DateTime.Now, "oficial", 900, 1000);
            cotizacion.Id = 1;

            var servicio = new Servicio("Fiat", DateTime.Now, null, true);
            servicio.Id = 1;

            _cuotaRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<CuotaServicio>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CuotaServicio?)null);
            _servicioRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(servicio);
            //_cotizacionDolarManagerMock.Setup(c => c.ObtenerYGuardarCotizacionAsync("oficial", It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync(cotizacion);
            _cuotaRepositoryMock.Setup(x => x.AddAsync(It.IsAny<CuotaServicio>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuotaServicio(1, DateTime.Now, 2000, 2, 1, 1));
            _mapperMock.Setup(x => x.Map<CuotaServicioResponse>(It.IsAny<CuotaServicio>()))
                .Returns(new CuotaServicioResponse { Id = 1 });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            _cuotaRepositoryMock.Verify(x => x.AddAsync(It.Is<CuotaServicio>(c => c.MontoARS == 2000), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
