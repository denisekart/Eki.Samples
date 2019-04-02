using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;

namespace SampleApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //in prod mode, the root will be as below and we will use static files from there
            services.AddSpaStaticFiles(cfg =>
                cfg.RootPath = "sample-client");

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //https://github.com/aspnet/Docs/issues/10753
            #region snippet_ChromeWorkaround
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sample}/{action=Get}/{id?}");
            });

            //when developing, this will proxy all of the webpack trafic to the proxy and hopefully omit any errors
            if(env.IsDevelopment())
                app.MapWhen(context => WebPackDevServerMatcher(context),
                    webpackDevServer =>
                    {
                        webpackDevServer.UseSpa(spa =>
                        {
                            spa.UseProxyToSpaDevelopmentServer(baseUri: "http://localhost:4200");
                        });
                    });
            //the /client will be out root
            app.Map(
                new PathString("/client"),
                client =>
                {
                    //not sure if i need this here
                    client.UsePathBase(new PathString("/client"));


                    //if we're in prod mode, we use the files from the built client
                    if (!env.IsDevelopment())
                        client.UseSpaStaticFiles();

                    client.UseSpa(cfg =>
                    {
                        //this is used only while developing
                        cfg.Options.SourcePath = "./../sample-client";
                        if (env.IsDevelopment())
                        {
                            //either we use our own client server
                            //cfg.UseAngularCliServer("start");

                            //or we just hook up to one
                            cfg.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                        }
                    });

                });
        }

        private bool WebPackDevServerMatcher(HttpContext context)
        {
            string pathString = context.Request.Path.ToString();
            return pathString.Contains(context.Request.PathBase.Add("/webpack-dev-server")) ||
                   context.Request.Path.StartsWithSegments("/__webpack_dev_server__") ||
                   context.Request.Path.StartsWithSegments("/sockjs-node");
        }
    }
}
