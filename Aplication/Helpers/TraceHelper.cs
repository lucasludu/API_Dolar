using Microsoft.AspNetCore.Http;

namespace Application.Helpers
{
    public static class TraceHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static string GetTraceId()
        {
            return _httpContextAccessor?.HttpContext?.Items["TraceId"]?.ToString()
                ?? System.Diagnostics.Activity.Current?.TraceId.ToString()
                ?? Guid.NewGuid().ToString();
        }
    }
}
