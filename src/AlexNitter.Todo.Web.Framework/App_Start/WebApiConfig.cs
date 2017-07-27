using AlexNitter.Todo.WebFramework.PipelineModules;
using System.Web.Http;

namespace AlexNitter.Todo.Web.Framework
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // WebApiAuthenticationModule zum Authentifizieren des Users anhand der SessionId im Cookie in die WebApi-Pipeline einhängen
            config.MessageHandlers.Add(new WebApiAuthenticationModule());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
