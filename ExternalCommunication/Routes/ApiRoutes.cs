namespace ExternalCommunication.Routes
{
    public static class ApiRoutes
    {
        public static string ArgentinaDatosBaseUrl = "https://api.argentinadatos.com/v1";
        public static string DolarBaseUrl = "https://dolarapi.com/v1";

        public static string GetEstadoApi 
            => $"{ArgentinaDatosBaseUrl}/estado";
        public static string GetCotizacionesHistoricasUSD 
            => $"{ArgentinaDatosBaseUrl}/cotizaciones/dolares";
        public static string GetCotizacionsTipoFecha(string tipo, DateTime fecha) 
            => $"{ArgentinaDatosBaseUrl}/cotizaciones/dolares/{tipo.ToLower()}/{fecha.ToString("yyyy/MM/dd")}";
        
        public static string GetFeriadosByYear(int year) 
            => $"{ArgentinaDatosBaseUrl}/feriados/{year}";
        
        public static string GetDolaresToday 
            => $"{DolarBaseUrl}/dolares";
        public static string GetDolaresTodayByTipo(string tipo) 
            => $"{DolarBaseUrl}/dolares/{tipo.ToLower()}";

        public static string GetCotizacionesOtrasMonedas 
            => $"{DolarBaseUrl}/cotizaciones";

        public static string GetCotizacionesOtrasMonedasByTipo(string tipo)
            => $"{DolarBaseUrl}/cotizaciones/{tipo.ToLower()}";
    }
}
