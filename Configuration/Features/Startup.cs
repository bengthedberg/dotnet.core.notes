using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore;

namespace Features
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
         
            app.UseRequestLocalization();

            app.Run(context =>
            {
                
                var str = string.Empty;
                str += "Connection" + System.Environment.NewLine;
                var connection = context.Features.Get<IHttpConnectionFeature>();
                str += $"Local IP:Port  : {connection.LocalIpAddress}:{connection.LocalPort}" + System.Environment.NewLine;
                str += $"Remote IP:Port : {connection.RemoteIpAddress}:{connection.RemotePort}" + System.Environment.NewLine;
                str += $"Connection Id  : {connection.ConnectionId}" + System.Environment.NewLine;
                
                // Needs app.UseRequestLocalization();
                str += System.Environment.NewLine + "Culture" + System.Environment.NewLine;
                var cultureFeature = context.Features.Get<IRequestCultureFeature>();
                str += $"Request culture : {cultureFeature.RequestCulture.Culture.EnglishName}" + System.Environment.NewLine;


                 str += System.Environment.NewLine + "Server Address" + System.Environment.NewLine;
                var serverAddress = app.ServerFeatures.Get<IServerAddressesFeature>();
                foreach(var a in serverAddress.Addresses)
                {
                    str += $"{a}" + System.Environment.NewLine;;
                }

                return context.Response.WriteAsync($"{str}");
            });
        }
    }
}
