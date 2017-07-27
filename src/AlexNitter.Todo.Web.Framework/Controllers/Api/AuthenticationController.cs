using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Services;
using System;
using System.Web;
using System.Web.Http;

namespace AlexNitter.Todo.WebFramework.Controllers.Api
{
    public class AuthenticationController : ApiController
    {
        private AuthenticationService _authService = new AuthenticationService();

        [HttpPost]
        [Route("api/Login")]
        public IHttpActionResult ApiLogin([FromBody]LoginRequest loginRequest)
        {
            // Prüfe die üebergenen Credentials und erzeuge im Erfolgsfall eine neue Session
            var loginResponse = _authService.Login(loginRequest);

            if (loginResponse.Success)
            {
                // Wenn die Authentifizierung erfolgreich war, liefere die SessionId als Text im Body der HTTP-Response zurück
                return Ok(loginResponse.SessionCreated.Id);
            }
            else
            {
                // Wenn die Authentifizierung nicht erfolgreich war, liefere den HTTP-Statuscode 401 zurück
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("api/Logout")]
        [Authorize] // Das Authorie-Attribut zeigt an, dass diese Route nur von autorisierten Usern aufgerufen werden darf. Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
        public IHttpActionResult ApiLogout()
        {
            var authHeader = HttpContext.Current.Request.Headers["Authorization"];

            if (!String.IsNullOrEmpty(authHeader))
            {
                // Wenn der Request einen Authorization-Header mit einer SessionId enthält, setze die Session auf inaktiv
                _authService.Logout(authHeader);
            }

            // Gib in jedem Fall den HTTP-Statuscode 200 zurück
            return Ok();
        }
    }
}