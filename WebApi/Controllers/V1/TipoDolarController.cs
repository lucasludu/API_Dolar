using Application.DTOs._tipoDolar.Request;
using Application.Features._tipoCambio.Command.InsertNewTipoDolarCommands;
using Application.Features._tipoCambio.Queries.GetAllTipoDolarQueries;
using Application.Features._tipoCambio.Queries.GetTipoCambioByNameQueries;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    /// <summary>
    /// Controlador para la gestión de tipos de cambio.
    /// </summary>
    /// <remarks>
    /// Permite registrar nuevos tipos de dólar, obtener el listado completo y buscar por nombre.
    /// </remarks>
    [ApiVersion("1.0")]
    public class TipoDolarController : BaseApiController
    {
        /// <summary>
        /// Registra un nuevo tipo de dólar en el sistema.
        /// </summary>
        /// <param name="request">Datos del tipo de dólar a registrar.</param>
        /// <returns>Resultado de la operación con confirmación o mensaje de error.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTipoDolar(TipoDolarRequest request)
        {
            return Ok(await Mediator.Send(new InsertNewTipoDolarCommand(request)));

        }

        /// <summary>
        /// Obtiene todos los tipos de dólar registrados.
        /// </summary>
        /// <returns>Listado completo de tipos de cambio disponibles.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllTipoDolarQuery()));
        }

        /// <summary>
        /// Busca un tipo de dólar por su nombre.
        /// </summary>
        /// <param name="name">Nombre del tipo de dólar a buscar (ej. "Oficial", "Blue").</param>
        /// <returns>Datos del tipo de cambio encontrado o mensaje de error si no existe.</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await Mediator.Send(new GetTipoCambioByNameQuery(name)));
        }
    }
}
