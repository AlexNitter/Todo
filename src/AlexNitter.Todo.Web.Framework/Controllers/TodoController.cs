using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.Controllers
{
    public class TodoController : Controller
    {
        [Route("Todo")]
        public ActionResult Index()
        {
            return View();
        }
    }
}