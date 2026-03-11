using Application.DTOs._servicio.Request;
using Application.DTOs._servicio.Response;
using Application.Features._servicio.Command.CreateServicioCommands;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Ardalis.Specification;

namespace Application.UnitTests.Features._servicio.Command.CreateServicioCommands
{
    public class CreateServicioCommandHandlerTests
    {
        private readonly Mock<IRepositoryAsync<Servicio>> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<CreateServicioCommand>> _loggerMock;
        private readonly CreateServicioCommandHandler _handler;

        public CreateServicioCommandHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryAsync<Servicio>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CreateServicioCommand>>();
            _handler = new CreateServicioCommandHandler(
                _repositoryMock.Object, 
                _unitOfWorkMock.Object, 
                _mapperMock.Object, 
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Add_Servicio_And_Commit_Transaction()
        {
            // Arrange
            var request = new ServicioRequest
            {
                Nombre = "Servicio de Prueba",
                FechaInicio = DateTime.Now,
                Activo = true
            };
            var command = new CreateServicioCommand(request);
            var servicio = new Servicio("Servicio de Prueba", DateTime.Now, null, true);
            var response = new ServicioResponse { Id = 1, Nombre = "Servicio de Prueba" };

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Servicio>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Servicio)null);
            _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Servicio>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(servicio);
            _mapperMock.Setup(x => x.Map<ServicioResponse>(It.IsAny<Servicio>()))
                .Returns(response);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Servicio>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_If_Servicio_Exists()
        {
            // Arrange
            var request = new ServicioRequest { Nombre = "Existente" };
            var command = new CreateServicioCommand(request);
            var servicioExistente = new Servicio("Existente", DateTime.Now, null, true);

            _repositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<ISpecification<Servicio>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(servicioExistente);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Application.Exceptions.ApiException>().WithMessage("El servicio ya existe.");
        }
    }
}
