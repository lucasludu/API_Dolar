using Application.DTOs._cotizaciones.Request;
using Application.DTOs._feriado.Request;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.V1
{
    /// <summary>
    /// Proporciona endpoints para obtener información sobre cotizaciones, incluyendo tipos de cambio del dólar, datos de feriados y otras monedas.
    /// Este controlador actúa como una puerta de enlace de la API para acceder a datos financieros desde el servicio subyacente.
    /// </summary>
    /// <remarks>
    /// El controlador expone varios endpoints para recuperar datos de cotizaciones de distintas monedas y períodos de tiempo.
    /// Incluye endpoints para:
    /// - Obtener el estado actual de la API.
    /// - Consultar cotizaciones del dólar para hoy o fechas específicas.
    /// - Consultar cotizaciones de otras monedas.
    /// - Obtener datos de feriados para un año determinado.
    /// 
    /// Cada endpoint valida los parámetros de entrada y devuelve respuestas HTTP apropiadas, como <see cref="BadRequest"/> para entradas inválidas o <see cref="NotFound"/> cuando no hay datos disponibles.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CotizacionDolarController : ControllerBase
    {
        private readonly IArgentidaDatosService _service;

        public CotizacionDolarController(IArgentidaDatosService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene el estado actual de la API de cotizaciones.
        /// </summary>
        /// <returns>Estado de disponibilidad y funcionamiento del servicio.</returns>
        [HttpGet("estado")]
        public async Task<IActionResult> GetEstadoApi()
        {
            var estado = await _service.GetEstadoApiAsync();
            return Ok(estado);
        }

        /// <summary>
        /// Obtiene todas las cotizaciones del dólar disponibles.
        /// </summary>
        /// <returns>Lista de tipos de dólar y sus valores actuales.</returns>
        [HttpGet("dolares")]
        public async Task<IActionResult> GetCotizacionesDolar()
        {
            var cotizaciones = await _service.GetCotizacionesDolarAsync();
            return Ok(cotizaciones);
        }

        /// <summary>
        /// Obtiene la cotización de un tipo de dólar específico en una fecha determinada.
        /// </summary>
        /// <param name="request">Parámetros que incluyen tipo de dólar y fecha.</param>
        /// <returns>Cotización correspondiente o error si no se encuentra.</returns>
        [HttpGet("dolares/por-fecha")]
        public async Task<IActionResult> GetCotizacionPorFecha([FromQuery]CotizacionesRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Tipo))
                return BadRequest("El tipo de dólar es obligatorio.");

            var cotizacion = await _service.GetCotizacionPorTipoYFechaAsync(request.Tipo, request.Fecha);

            if (cotizacion == null)
                return NotFound($"No se encontró cotización para {request.Tipo} en {request.Fecha:yyyy-MM-dd}");

            return Ok(cotizacion);
        }

        /// <summary>
        /// Obtiene los feriados nacionales para un año específico.
        /// </summary>
        /// <param name="request">Año para el cual se desean consultar los feriados.</param>
        /// <returns>Lista de feriados o error si el año no es válido.</returns>
        [HttpGet("feriados")]
        public async Task<IActionResult> GetFeriados([FromQuery]FeriadoRequest request)
        {
            if (request.Year < 2016 || request.Year > 2025)
                return BadRequest("El año debe estar entre 2016 y 2025.");

            var feriados = await _service.GetFeriadosAsync(request.Year);
            return Ok(feriados);
        }

        /// <summary>
        /// Obtiene las cotizaciones del dólar correspondientes al día de hoy.
        /// </summary>
        /// <returns>Lista de tipos de dólar y sus valores actuales para hoy.</returns>
        [HttpGet("dolares/today")]
        public async Task<IActionResult> GetCotizacionesDolarToday()
        {
            var cotizaciones = await _service.GetCotizacionesDolarTodayAsync();
            return Ok(cotizaciones);
        }

        /// <summary>
        /// Obtiene la cotización de un tipo de dólar específico para el día de hoy.
        /// </summary>
        /// <param name="name">Nombre del tipo de dólar (ej. oficial, blue, etc.).</param>
        /// <returns>Cotización correspondiente o error si no se encuentra.</returns>
        [HttpGet("dolares/{name}")]
        public async Task<IActionResult> GetCotizacionPorTipo([FromRoute] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("El tipo de dólar es obligatorio.");
            var result = await _service.GetCotizacionesDolarTodayByTypoAsync(name);
            if (result == null)
                return NotFound($"No se encontró cotización para {name} en la fecha de hoy");
            return Ok(result);
        }

        /// <summary>
        /// Obtiene las cotizaciones de otras monedas distintas al dólar.
        /// </summary>
        /// <returns>Lista de monedas extranjeras y sus valores actuales.</returns>
        [HttpGet("otras-monedas")]
        public async Task<IActionResult> GetOtrasMonedasCotizaciones()
        {
            var cotizaciones = await _service.GetOtrasMonedasCotizaciones();
            return Ok(cotizaciones);
        }

        /// <summary>
        /// Obtiene la cotización de una moneda específica distinta al dólar.
        /// </summary>
        /// <param name="name">Nombre de la moneda (ej. euro, real, etc.).</param>
        /// <returns>Cotización correspondiente o error si no se encuentra.</returns>
        [HttpGet("otras-monedas/{name}")]
        public async Task<IActionResult> GetOtrasMonedasCotizacionPorTipo([FromRoute] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("El tipo de dólar es obligatorio.");
            var result = await _service.GetOtrasMonedasCotizacionesByTypoAsync(name);
            if (result == null)
                return NotFound($"No se encontró cotización para {name} en la fecha de hoy");
            return Ok(result);
        }

    }
}
