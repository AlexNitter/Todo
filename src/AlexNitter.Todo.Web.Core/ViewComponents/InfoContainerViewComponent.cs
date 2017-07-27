using AlexNitter.Todo.Lib.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlexNitter.Todo.Web.Core.ViewComponents
{
    public class InfoContainerViewComponent : ViewComponent
    {
        // Infos zu ViewComponentens: https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components

        public IViewComponentResult Invoke()
        {
            var viewModel = new InfoContainerViewModel();

            viewModel.Eingeloggt = HttpContext.User.Identity.IsAuthenticated;

            if(viewModel.Eingeloggt)
            {
                viewModel.Username = HttpContext.User.Identity.Name;
            }

            return View("~/Views/Shared/Components/InfoContainer.cshtml", viewModel);
        }
    }
}
