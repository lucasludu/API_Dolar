using Application.DTOs._cotizaciones.Response;
using Application.DTOs._feriado.Response;
using Application.DTOs._state.Response;
using Application.Interfaces;
using ExternalCommunication.Routes;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ExternalCommunication.Services
{
    public class ArgentinaDatosService : IArgentidaDatosService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ArgentinaDatosService> _logger;

        public ArgentinaDatosService(HttpClient httpClient, ILogger<ArgentinaDatosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<List<CotizacionesResponse>> GetCotizacionesDolarAsync()
        {
            try
            {
                _logger.LogInformation("Fetching Cotizaciones Dolar Historicas...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetCotizacionesHistoricasUSD);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizaciones = JsonSerializer.Deserialize<List<CotizacionesResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Cotizaciones Dolar Historicas fetched successfully. Count: {cotizaciones?.Count ?? 0}");
                return cotizaciones ?? new List<CotizacionesResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return new List<CotizacionesResponse>();
            }
        }

        public async Task<List<CotizacionesResponse>> GetCotizacionesDolarTodayAsync()
        {
            try
            {
                _logger.LogInformation("Fetching Cotizaciones Dolar Today...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetDolaresToday);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizaciones = JsonSerializer.Deserialize<List<CotizacionesResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Cotizaciones Dolar Today fetched successfully. Count: {cotizaciones?.Count ?? 0}");
                return cotizaciones ?? new List<CotizacionesResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return new List<CotizacionesResponse>();
            }
        }

        public async Task<CotizacionesResponse?> GetCotizacionesDolarTodayByTypoAsync(string tipo)
        {
            try
            {
                _logger.LogInformation($"Fetching Cotizaciones Dolar Today by Tipo: {tipo}...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetDolaresTodayByTipo(tipo));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizacion = JsonSerializer.Deserialize<CotizacionesResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Cotizaciones Dolar Today by Tipo: {tipo} fetched successfully.");
                return cotizacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return null;
            }
        }

        public async Task<CotizacionesResponse?> GetCotizacionPorTipoYFechaAsync(string tipo, DateTime fecha)
        {
            try
            {
                _logger.LogInformation($"Fetching Cotizacion for Tipo: {tipo} and Fecha: {fecha.ToShortDateString()}...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetCotizacionsTipoFecha(tipo, fecha));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizacion = JsonSerializer.Deserialize<CotizacionesResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Cotizacion for Tipo: {tipo} and Fecha: {fecha.ToShortDateString()} fetched successfully.");
                return cotizacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return null;
            }
        }

        public async Task<EstadoResponse> GetEstadoApiAsync()
        {
            try
            {
                _logger.LogInformation("Fetching Estado API...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetEstadoApi);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var estado = JsonSerializer.Deserialize<EstadoResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Estado API fetched successfully.");
                return estado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return null;
            }
        }

        public async Task<List<FeriadoResponse>> GetFeriadosAsync(int year)
        {
            try
            {
                _logger.LogInformation($"Fetching Feriados for Year: {year}...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetFeriadosByYear(year));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var feriados = JsonSerializer.Deserialize<List<FeriadoResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                _logger.LogInformation($"Feriados for Year: {year} fetched successfully. Count: {feriados?.Count ?? 0}");
                return feriados ?? new List<FeriadoResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return new List<FeriadoResponse>();
            }
        }

        public async Task<List<CotizacionesResponse>> GetOtrasMonedasCotizaciones()
        {
            try
            {
                _logger.LogInformation("Fetching Cotizaciones Otras Monedas...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetCotizacionesOtrasMonedas);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizaciones = JsonSerializer.Deserialize<List<CotizacionesResponse>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation($"Cotizaciones Otras Monedas fetched successfully. Count: {cotizaciones?.Count ?? 0}");
                return cotizaciones ?? new List<CotizacionesResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return new List<CotizacionesResponse>();
            }
        }

        public async Task<CotizacionesResponse> GetOtrasMonedasCotizacionesByTypoAsync(string typo)
        {
            try
            {
                _logger.LogInformation($"Fetching Cotizaciones Otras Monedas by Tipo: {typo}...");
                var response = await _httpClient.GetAsync(ApiRoutes.GetCotizacionesOtrasMonedasByTipo(typo));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var cotizacion = JsonSerializer.Deserialize<CotizacionesResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                
                _logger.LogInformation($"Cotizaciones Otras Monedas by Tipo: {typo} fetched successfully.");
                return cotizacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error en el servicio:\t{ex.Message}");
                return null;
            }
        }
    }
}
