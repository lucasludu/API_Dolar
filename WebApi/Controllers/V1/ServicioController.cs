using Application.DTOs._cuotaServicio.Request;
using Application.DTOs._servicio.Request;
using Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands;
using Application.Features._servicio.Command.CreateServicioCommands;
using Application.Features._servicio.Command.DeleteServicioCommands;
using Application.Features._servicio.Command.UpdateServicioCommands;
using Application.Features._servicio.Queries.GetAllServiciosQueries;
using Application.Features._servicio.Queries.GetServicioByNameQueries;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    /// <summary>
    /// Controlador para la gestión de servicios.
    /// </summary>
    /// <remarks>
    /// Permite crear nuevos servicios, obtener el listado completo y buscar por nombre.
    /// </remarks>
    [ApiVersion("1.0")]
    public class ServicioController : BaseApiController
    {
        /// <summary>
        /// Crea un nuevo servicio en el sistema.
        /// </summary>
        /// <param name="request">Datos del servicio a registrar.</param>
        /// <returns>Resultado de la operación con el ID generado o mensaje de estado.</returns>
        [HttpPost("CrearServicio")]
        public async Task<IActionResult> CrearServicio([FromBody] ServicioRequest request)
        {
            return Ok(await Mediator.Send(new CreateServicioCommand(request)));
        }

        /// <summary>
        /// Obtiene todos los servicios registrados.
        /// </summary>
        /// <returns>Listado completo de servicios disponibles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllServiciosQuery()));
        }

        /// <summary>
        /// Busca un servicio por su nombre.
        /// </summary>
        /// <param name="name">Nombre del servicio a buscar.</param>
        /// <returns>Datos del servicio encontrado o mensaje de error si no existe.</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await Mediator.Send(new GetServicioByNameQuery(name)));
        }

        /// <summary>
        /// Modifica un servicio existente identificado por su ID.
        /// </summary>
        /// <param name="id">Identificador a buscar</param>
        /// <param name="request">Request a modificar</param>
        /// <returns></returns>
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateServicio(int id, [FromBody] ServicioRequest request)
        {
            return Ok(await Mediator.Send(new UpdateServicioCommand(id, request)));
        }


        /// <summary>
        /// Elimina un servicio existente identificado por su ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicio(int id)
        {
            return Ok(await Mediator.Send(new DeleteServicioCommand(id)));
        }
    }
}
