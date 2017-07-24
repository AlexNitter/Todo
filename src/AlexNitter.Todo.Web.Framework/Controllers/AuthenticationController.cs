using AlexNitter.Todo.Lib.DataModels;
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
    }
}