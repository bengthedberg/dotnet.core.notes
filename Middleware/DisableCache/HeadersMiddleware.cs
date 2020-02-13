using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DisableCache
{
    public class HeadersMiddleware
    {
        // A middleware classes should have a constructor that takes a 
        // `RequestDelegate` object, which represents the rest of the middleware pipeline
        private readonly RequestDelegate _next;
        public HeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Sets a function that should be called before the response is sent to the browser
            context.Response.OnStarting(() =>
            {
                // Disable cache
                // If a web server responds with Cache-Control: no-cache then a web browser or 
                // other caching system (intermediate proxies) must not use the response to satisfy 
                // subsequent requests without first checking with the originating server 
                // (this process is called validation). This header field is part of HTTP version 1.1, 
                // and is ignored by some caches and browsers
                context.Response.Headers["Cache-Control"] = "no-cache";
                return Task.CompletedTask;
            });
            // Continue with the rest of the pipeline
            await _next(context);
        }
    }
}
