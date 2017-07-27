using AlexNitter.Todo.WebFramework.PipelineModules;
using System.Web.Mvc;

namespace AlexNitter.Todo.Web.Framework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // MvcAuthenticationModule zum Authentifizieren des Users anhand der SessionId im Cookie in die MVC-Pipeline einhängen
            filters.Add(new MvcAuthenticationModule());
        }
    }
}