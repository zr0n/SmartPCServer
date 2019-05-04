using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SmartPCServer.Controllers;
using SmartPCServer.Services;
namespace SmartPCServer
{
    public class Startup
    {
        public static readonly string WebSocketEndpoint = "/ws";

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Set  Asp.net Compatibility Version (2.2) -- Nando
            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            //Dependency Injection -- Nando
            services.AddSingleton<IApiService, ApiService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //https-less -- Nando
            app.UseDeveloperExceptionPage();

            // if using https, uncoment these boys:

            /*
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            */

            app.UseMvc(); // -- Nando
            app.UseWebSockets(); // -- Nando

            // -- Nando (Accept web socket connection async)
            app.Use(
                async (context, next) =>
                {
                    if (context.Request.Path.Equals(WebSocketEndpoint))
                    {
                        if (context.WebSockets.IsWebSocketRequest)
                        {
                            WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
                            await WebSocketPipe(context, ws);
                        }
                    }
                });

        }

        // -- Nando
        private async Task WebSocketPipe(HttpContext context, WebSocket ws)
        {
            var handle = new WebSocketHandle(ws);
            WebSocketReceiveResult result = await ws.ReceiveAsync(new ArraySegment<byte>(handle.buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                // -- Nando - Build String Buffered
                await handle.ParseMessageReceived(result);
                result = await ws.ReceiveAsync(new ArraySegment<byte>(handle.buffer), CancellationToken.None);
            }

            await ws.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
