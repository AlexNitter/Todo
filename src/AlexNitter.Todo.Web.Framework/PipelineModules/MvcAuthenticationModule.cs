using System;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.PipelineModules
{
    /// <summary>
    /// Modul, das in die MVC-Pipeline eingehängt wird. Dieses Modul wird bei jedem HTTP-Request gegen die Anwendung aufgerufen, noch vor den MVC-Controllern.
    /// Dieses Modul prüft die SessionId des Cookies aus dem HTTP-Request, validiert sie und setzt im Erfolgsfall den User im HttpContext. 
    /// Dadurch wissen die MVC-Controller, dass der HTTP-Request von einem authentifizierten User erfolgt
    /// </summary>
    public class MvcAuthenticationModule : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            setUserBySessionId(filterContext);
        }

        private void setUserBySessionId(AuthorizationContext filterContext)
        {
            try
            {
                // Durchsuch die Cookies nach einem SessionId-Cookie
                var cookie = filterContext.RequestContext.HttpContext.Request.Cookies.Get("SessionId");

                if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
                {
                    var authModule = new BaseAuthenticationModule();

                    // Wenn ein SessionId-Cookie gefunden wurde, validiere sie und versuche den Principal zu setzen
                    authModule.SetPrincipalBySessionId(cookie.Value);
                }
            }
            catch (Exception ex)
            {
                // TODO: Exception loggen
            }
        }
    }
}