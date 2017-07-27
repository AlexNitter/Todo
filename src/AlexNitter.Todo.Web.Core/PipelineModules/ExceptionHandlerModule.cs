using AlexNitter.Todo.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AlexNitter.Todo.Web.Core.PipelineModules
{
    /// <summary>
    /// Modul zum Behandeln von Exceptions, die innerhalb der Pipeline auftreten
    /// </summary>
    public class ExceptionHandlerModule : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Loggen der Exception
            LoggingService.Log(context.Exception);

            // Weiterleitung auf eine benutzerfreundliche Fehlerseite
            context.Result = new RedirectResult("~/Error/500");
        }
    }
}
