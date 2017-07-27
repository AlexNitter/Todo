using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Services;
using AlexNitter.Todo.Lib.ViewModels;
using System;
using System.Web;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.Controllers.Mvc
{
    public class AuthenticationController : Controller
    {
        private AuthenticationService _authService = new AuthenticationService();

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
            var sessionId = HttpContext.Request.Cookies.Get("SessionId");

            if (sessionId != null && !String.IsNullOrEmpty(sessionId.Value))
            {
                // Session serverseitig deaktivieren
                _authService.Logout(sessionId.Value);

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

            var cookie = new HttpCookie(name, value)
            {
                Path = "/",
                HttpOnly = true,
                Expires = expires
            };

            Response.SetCookie(cookie);
        }
    }
}