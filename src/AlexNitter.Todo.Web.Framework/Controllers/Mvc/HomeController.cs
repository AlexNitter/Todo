using AlexNitter.Todo.Lib.ViewModels;
using System;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.Controllers.Mvc
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public ActionResult Startseite()
        {
            if (HttpContext.User.Identity.IsAuthenticated && !String.IsNullOrEmpty(HttpContext.User.Identity?.Name))
            {
                // Ist der User bereits authentifiziert, Weiterleitung auf die Todo-Seite
                return RedirectToAction("Index", "Todo");
            }
            else
            {
                // Ist der User noch nicht authentifiziert, Weiterleitung auf die Login-Seite
                return RedirectToAction("Login", "Authentication");
            }
        }

        [HttpGet]
        [Route("Error/{statuscode}")]
        public ActionResult ErrorPage(Int32 statuscode)
        {
            // Ausliefern einer benutzerfreundlichen Fehler-HTML-Seite, abhängig vom übergenen HTTP-Statuscode
            switch (statuscode)
            {
                case 401:
                    return View("~/Views/ErrorPages/401.cshtml");
                case 404:
                    return View("~/Views/ErrorPages/404.cshtml");
                default:
                    return View("~/Views/ErrorPages/Generic.cshtml", statuscode.ToString());
            }
        }
        
        [ChildActionOnly]
        public ActionResult InfoContainer()
        {
            var viewModel = new InfoContainerViewModel();

            viewModel.Eingeloggt = HttpContext.User.Identity.IsAuthenticated;

            if (viewModel.Eingeloggt)
            {
                viewModel.Username = HttpContext.User.Identity.Name;
            }

            return View("~/Views/Partial/_InfoContainer.cshtml", viewModel);
        }
    }
}