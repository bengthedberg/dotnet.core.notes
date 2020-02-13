using Microsoft.AspNetCore.Builder;

namespace DisableCache
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseDisabledCacheHeaders(
        this IApplicationBuilder app)
        {
            return app.UseMiddleware<HeadersMiddleware>();
        }
    }
}
