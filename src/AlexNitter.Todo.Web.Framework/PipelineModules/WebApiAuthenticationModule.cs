using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlexNitter.Todo.WebFramework.PipelineModules
{
    /// <summary>
    /// Modul, das in die WebApi-Pipeline eingehängt wird. Dieses Modul wird bei jedem HTTP-Request gegen die Anwendung aufgerufen, noch vor den WebApi-Controllern.
    /// Dieses Modul prüft die SessionId des Cookies aus dem HTTP-Request, validiert sie und setzt im Erfolgsfall den User im HttpContext. 
    /// Dadurch wissen die WebApi-Controller, dass der HTTP-Request von einem authentifizierten User erfolgt
    /// </summary>
    public class WebApiAuthenticationModule : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            setUserBySessionId(request);

            // Weitere Verarbeitung in der WebApi-Pipeline (WebApi-Controller)
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        private void setUserBySessionId(HttpRequestMessage request)
        {
            try
            {
                // Durchsuch die Cookies nach einem SessionId-Cookie
                var cookie = request.Headers.GetCookies("SessionId");

                if (cookie != null && cookie.Count == 1 && !String.IsNullOrEmpty(cookie[0].Cookies[0].Value))
                {
                    var authModule = new BaseAuthenticationModule();

                    // Wenn ein SessionId-Cookie gefunden wurde, validiere sie und versuche den Principal zu setzen
                    authModule.SetPrincipalBySessionId(cookie[0].Cookies[0].Value);
                }
            }
            catch (Exception ex)
            {
                // TODO: Exception loggen
            }
        }
    }
}