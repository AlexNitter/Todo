using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Services;
using AlexNitter.Todo.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet]
        [Route("Login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginRequest request)
        {
            return View();
        }

        [HttpPost]
        [Route("Logout")]
        public ActionResult Logout()
        {
            return View();
        }

        [HttpGet]
        [Route("Register")]
        public ActionResult Register()
        {
            return View(new BaseViewModel());
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterRequest request)
        {
            var service = new AuthenticationService();
            var result = service.Register(request);
            
            if(!result.Success)
            {
                return View(new BaseViewModel() { Message = result.Message });
            }
            else
            {
                return RedirectToRoute("Todo");
            }
        }
    }
}