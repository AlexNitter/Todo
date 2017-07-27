using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Services;
using AlexNitter.Todo.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AlexNitter.Todo.Web.Core.Controllers
{
    public class AuthenticationController : Controller
    {
        private AuthenticationService _authService = new AuthenticationService();

        #region API-Routen für die REST-Schnittstelle
        [HttpPost]
        [Route("api/Login")]
        public IActionResult ApiLogin([FromBody]LoginRequest loginRequest)
        {
            // Prüfe die üebergenen Credentials und erzeuge im Erfolgsfall eine neue Session
            var loginResponse = _authService.Login(loginRequest);

            if(loginResponse.Success)
            {
                // Wenn die Authentifizierung erfolgreich war, liefere die SessionId als Text im Body der HTTP-Response zurück
                return new OkObjectResult(loginResponse.SessionCreated.Id);
            }
            else
            {
                // Wenn die Authentifizierung nicht erfolgreich war, liefere den HTTP-Statuscode 401 zurück
                return new UnauthorizedResult(); 
            }
        }

        [HttpPost]
        [Route("api/Logout")]
        [Authorize] // Das Authorie-Attribut zeigt an, dass diese Route nur von autorisierten Usern aufgerufen werden darf. Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
        public IActionResult ApiLogout()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];

            if (authHeader.Count == 1 && !String.IsNullOrEmpty(authHeader[0]))
            {
                // Wenn der Request einen Authorization-Header mit einer SessionId enthält, setze die Session auf inaktiv
                _authService.Logout(authHeader[0]);
            }

            // Gib in jedem Fall den HTTP-Statuscode 200 zurück
            return Ok();
        }
        #endregion


        #region MVC-Routen für die HTML-Schnittstelle
        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            // Liefere beim HTTP-GET lediglich das HTML-Dokument zurück
            return View("~/Views/Authentication/Login.cshtml");
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginRequest request)
        {
            // Nimm die HTTP-POST-Daten entgegen und versuche sie zu authentifizieren
            var loginResponse = _authService.Login(request);

            if (loginResponse.Success)
            {
                // War die Authentifizierung erfolgreich, setze den Cookie
                setCookie("SessionId", loginResponse.SessionCreated.Id);

                // Weiterleitung zur Todo-Seite
                return RedirectToAction("Index", "Todo");
            }
            else
            {
                // War die Authentifizierung nicht erfolgreich, erzeuge das Model-Objekt mit der Fehlermeldung
                var model = new BaseViewModel() { Message = loginResponse.Message };

                // Gib erneut das Login-HTML-Dokument zurück mit dem entsprechenden Model
                return View("~/Views/Authentication/Login.cshtml", model);
            }
        }

        [HttpGet]
        [Route("Logout")]
        [Authorize] // Das Authorie-Attribut zeigt an, dass diese Route nur von autorisierten Usern aufgerufen werden darf. Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
        public ActionResult Logout()
        {
            var sessionId = HttpContext.Request.Cookies["SessionId"];

            if (sessionId != null && !String.IsNullOrEmpty(sessionId))
            {
                // Session serverseitig deaktivieren
                _authService.Logout(sessionId);

                // Session clientseitig deaktivieren
                setCookie("SessionId", "", true);
            }

            // Weiterleitung zur Startseite
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("Register")]
        public ActionResult Register()
        {
            // Liefere beim HTTP-GET lediglich das HTML-Dokument zurück
            return View("~/Views/Authentication/Register.cshtml", new BaseViewModel());
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterRequest request)
        {
            // Nimm die HTTP-POST-Daten entgegen und registriere den neuen User
            var result = _authService.Register(request);

            if (!result.Success)
            {
                // War die Registrierung nicht erfolgreich, erzeuge das Model-Objekt mit der Fehlermeldung
                // Gib erneut das Login-HTML-Dokument zurück mit dem entsprechenden Model
                return View("~/Views/Authentication/Register.cshtml", new BaseViewModel() { Message = result.Message });
            }
            else
            {
                // Weiterleitung zur Login-Seite
                return RedirectToAction("Login", "Authentication");
            }
        }
        #endregion

        private void setCookie(String name, String value, Boolean removeCookie = false)
        {
            // Ein Cookie kann nicht explizit gelöscht werden - stattdessen muss das Ablaufdatum (Expires) auf einen Wert in der Vergangenheit gesetzt werden

            DateTime expires;
            if (!removeCookie)
            {
                expires = DateTime.Now.AddDays(1);
            }
            else
            {
                expires = DateTime.Now.AddDays(-1);
            }

            var cookieOptions = new CookieOptions()
            {
                Path = "/",
                HttpOnly = true,
                Expires = expires
            };

            Response.Cookies.Append(name, value, cookieOptions);
        }
    }
}
