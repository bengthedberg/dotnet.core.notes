using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SimpleUseExtension
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // The Use extension takes a lambda with HttpContext (context) and Func<Task> (next) parameters.
            app.Use(async (context, next) =>
            {
                // The StartsWithSegments method looks for the provided segment in the current path.
                if (context.Request.Path.StartsWithSegments("/time"))
                {
                    // If the path matches, generate a response, and short-circuit the pipeline
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(DateTime.UtcNow.ToString());
                }
                else
                {
                    // If the path doesn’t match, call the next middleware in the pipeline, in this case the UseRouting Middleware.
                    await next();
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Simple Middleware using the Use Extension!");
                });
            });
        }
    }
}
