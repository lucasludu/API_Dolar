using Application.DTOs._cuotaServicio.Request;
using Application.Features._cuotaServicio.Command.CargarCuotasServicioCommands;
using Application.Features._cuotaServicio.Command.DeleteCuotaServicioCommands;
using Application.Features._cuotaServicio.Command.UpdateCuotaServicioCommands;
using Application.Features._cuotaServicio.Queries.GetAllCuotasServicioQueries;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    /// <summary>
    /// Controlador para gestionar cuotas de servicio.
    /// </summary>
    /// <remarks>
    /// Expone endpoints para cargar, consultar, actualizar y eliminar cuotas.
    /// </remarks>
    [ApiVersion("1.0")]
    public class CuotaServicioController : BaseApiController
    {
        /// <summary>
        /// Carga una nueva cuota de servicio.
        /// </summary>
        /// <param name="cuotaServicioDto">Datos de la cuota a registrar.</param>
        /// <returns>Resultado de la operación con el ID generado o mensaje de estado.</returns>
        [HttpPost("CargarCuotasServicio")]
        public async Task<IActionResult> CargarCuotasServicio([FromBody] CuotaServicioRequest cuotaServicioDto)
        {
            // Lógica para cargar las cuotas de servicio
            return Ok(await Mediator.Send(new CargarCuotasServicioCommand(cuotaServicioDto)));
        }

        /// <summary>
        /// Obtiene todas las cuotas de servicio según filtros opcionales.
        /// </summary>
        /// <param name="parameters">Parámetros de búsqueda y paginación.</param>
        /// <returns>Listado de cuotas con metadatos asociados.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]GetAllCuotasServicioParameters parameters)
        {
            return Ok(await Mediator.Send(new GetAllCuotasServicioQuery(parameters)));
        }

        /// <summary>
        /// Elimina una cuota de servicio por su identificador.
        /// </summary>
        /// <param name="id">Identificador único de la cuota.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCuotaServicio(int id)
        {
            return Ok(await Mediator.Send(new DeleteCuotaServicioCommand(id)));
        }

        /// <summary>
        /// Actualiza los datos de una cuota de servicio existente.
        /// </summary>
        /// <param name="id">Identificador de la cuota a modificar.</param>
        /// <param name="cuotaServicioDto">Datos actualizados de la cuota.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPatch("update/{id}")]
        public async Task<IActionResult> UpdateCuotaServicio(int id, [FromBody] CuotaServicioRequest cuotaServicioDto)
        {
            return Ok(await Mediator.Send(new UpdateCuotaServicioCommand(id, cuotaServicioDto)));
        }

    }
}
