using AlexNitter.Todo.Lib;
using AlexNitter.Todo.Lib.Services;
using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlexNitter.Todo.Web.Framework
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Setzen der anwendungsspezifischen Konfiguration
            setConfig();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        /// <summary>
        /// Setzt die anwendungsspezifischen Konfigurationswerte anhand der appconfig.json Datei
        /// </summary>
        private void setConfig()
        {
            Config.InitializeConfig(
                connectionstring: ConfigurationManager.AppSettings["Connectionstring"],
                logFileDirectory: ConfigurationManager.AppSettings["LogFileDirectory"]);
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LoggingService.Log(exception);
            HttpContext.Current.Response.Redirect("~/Error/500");
        }
    }
}