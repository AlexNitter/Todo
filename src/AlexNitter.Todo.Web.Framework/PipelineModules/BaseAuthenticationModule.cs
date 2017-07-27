using AlexNitter.Todo.Lib.DataLayer.Repositories;
using System;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace AlexNitter.Todo.WebFramework.PipelineModules
{
    /// <summary>
    /// Basis-Klasse sowohl für das MVC- als auch für das WebApi-Modul. Nimmt eine SessionId entgegen, validiert die Session und 
    /// liefert im Erfolgsfall ein IPrincipal zurück, dessen Name den Usernamen der Session enthält
    /// </summary>
    public class BaseAuthenticationModule
    {
        /// <summary>
        /// Nimmt eine SessionId entgegen, validiert die Session und setzt im Erfolgsfall den Principal im HttpContext, sodass der User als authentifiziert gilt
        /// </summary>
        /// <param name="sessionId">SessionId aus dem Cookie des HTTP-Requests</param>
        public void SetPrincipalBySessionId(String sessionId)
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

                        HttpContext.Current.User = principal;
                        Thread.CurrentPrincipal = principal;
                    }
                }
            }
        }
    }
}