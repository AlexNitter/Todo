using AlexNitter.Todo.Lib;
using AlexNitter.Todo.Web.Core.PipelineModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace AlexNitter.Todo.Web.Core
{
    /// <summary>
    /// Konfiguration der Pipeline
    /// </summary>
    public class Startup
    {
        // Infos zur Startup: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup

        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Infos zu Filters: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters

            services.AddMvc(config =>
            {
                // Modul zum Authentifizieren des Users anhand der im Cookie übertragenen SessionId
                config.Filters.Add(new AuthenticationModule());

                // Modul zum Loggen aufgetretener unbehandelter Exceptions
                config.Filters.Add(new ExceptionHandlerModule());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Setzen der anwendungsspezifischen Konfiguration
            setConfig();

            app.UseForwardedHeaders(new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Im Fehlerfall auf die Route weiterleiten, um eine benutzerfreundliche HTML-Seite auszuliefern (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            // Ausliefern von statischen Dokumenten wie CSS, JavaScript etc. inklusive Konfiguration der erlaubten Routen
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),
                RequestPath = new PathString("/wwwroot")
            });

            // Erlauben von MVC-Routen
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Setzt die anwendungsspezifischen Konfigurationswerte anhand der appconfig.json Datei
        /// </summary>
        private void setConfig()
        {
            // Infos zur appsettings.json: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration, https://joonasw.net/view/aspnet-core-2-configuration-changes

            Config.InitializeConfig(
                connectionstring: Configuration["TodoAppConfigs:Connectionstring"],
                logFileDirectory: Configuration["TodoAppConfigs:LogFileDirectory"]);
        }
    }
}
