using AlexNitter.Todo.Lib.DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Principal;
using System.Threading;

namespace AlexNitter.Todo.Web.Core.PipelineModules
{
    /// <summary>
    /// Modul zum Authentifizieren
    /// Nimmt eine SessionId entgegen, validiert die Session und setzt im Erfolgsfall die User-Eigenschaft der HttpContext-Umgebungsvariable, dessen Name den Usernamen der Session enthält
    /// </summary>
    public class AuthenticationModule : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Durchsuch die Cookies nach einem SessionId-Cookie
            var sessionId = context.HttpContext.Request.Cookies["SessionId"];

            if(String.IsNullOrEmpty(sessionId))
            {
                // Wenn im Cookie keine SessionId war, durchsuche die HTTP-Header nach einer SessionId
                var authHeader = context.HttpContext.Request.Headers["Authorization"];

                if(authHeader.Count == 1 && !String.IsNullOrEmpty(authHeader[0]))
                {
                    sessionId = authHeader[0];
                }
            }

            if (!String.IsNullOrEmpty(sessionId))
            {
                setPrincipalBySessionId(sessionId, context.HttpContext);
            }
        }

        /// <summary>
        /// Nimmt eine SessionId entgegen, validiert die Session und setzt im Erfolgsfall den Principal im HttpContext, sodass der User als authentifiziert gilt
        /// </summary>
        /// <param name="sessionId">SessionId aus dem Cookie des HTTP-Requests</param>
        private void setPrincipalBySessionId(String sessionId, HttpContext context)
        {
            if (sessionId != null && !String.IsNullOrEmpty(sessionId))
            {
                // Wenn eine SessionId übergeben wurde, validiere sie
                var authService = new AlexNitter.Todo.Lib.Services.AuthenticationService();
                var session = authService.ValidateSession(sessionId);

                if (session != null)
                {
                    // Wenn die SessionId valide ist, prüfe ob es einen existierenden User dazu gibt
                    var userRepo = new UserRepository();
                    var user = userRepo.FindById(session.UserId);

                    if (user != null)
                    {
                        // Wenn der User zur Session existiert, setze ihn in der "User-"Eigenschaft des HttpContext
                        // Diese Eigenschaft ist der Indikator des "Authorize"-Attributs bei den Controllern, ob der User authorisiert ist oder nicht
                        var principal = new GenericPrincipal(new GenericIdentity(user.Username), null);

                        context.User = principal;
                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
        }
    }
}
