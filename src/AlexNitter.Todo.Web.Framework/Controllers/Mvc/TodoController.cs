using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Entities;
using AlexNitter.Todo.Lib.Services;
using AlexNitter.Todo.Lib.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace AlexNitter.Todo.WebFramework.Controllers.Mvc
{
    // Das Authorie-Attribut zeigt an, dass diese Routen nur von autorisierten Usern aufgerufen werden dürfen.
    // Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
    [Authorize]
    public class TodoController : Controller
    {
        private TodoService _todoService = new TodoService();

        [HttpGet]
        [Route("Todo/{*selectedListId}")]
        public ActionResult Index(Nullable<Int32> selectedListId)
        {
            // Hole zunächst alle Listen des authentifizierten Users
            var alleListen = _todoService.GetListenFromUser(HttpContext.User.Identity.Name);

            TodoListe selectedListe = null;

            // Hole als nächstes die vom User ausgewählte Liste. Wurde keine gewählt, nimm die erste
            if (alleListen != null && alleListen.Count > 0)
            {
                if (selectedListId != null)
                {
                    selectedListe = alleListen.FirstOrDefault(x => x.Id == selectedListId);

                    if (selectedListe == null)
                    {
                        selectedListe = alleListen[0];
                    }
                }
                else
                {
                    selectedListe = alleListen[0];
                }
            }

            var viewModel = new TodoViewModel()
            {
                Listen = alleListen,
                SelectedListe = selectedListe

            };

            // Gib das Todo-HTML-Dokument zurück mit dem entsprechenden Model
            return View("~/Views/Todo/Index.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Todo/Liste/Neu")]
        public ActionResult NeueListe(TodoListe entity)
        {
            // Speichere die übergebene Liste in der Datenbank
            _todoService.CreateNewListe(entity, HttpContext.User.Identity.Name);

            // Weiterleitung auf die Todo-Seite, mit der neu erzeugten Liste als ausgewählte Liste
            return RedirectToAction("Index", "Todo", new { selectedListId = entity.Id });
        }

        [HttpGet]
        [Route("Todo/Liste/Delete/{listeId}")]
        public ActionResult ListeLoeschen(Int32 listeId)
        {
            // Lösche die übergebene Liste aus der Datenbank
            _todoService.DeleteListe(listeId);

            // Weiterleitung auf die Todo-Seite, ohne Angabe zu einer ausgewählten Liste
            return RedirectToAction("Index", "Todo");
        }

        [HttpPost]
        [Route("Todo/Item/Neu")]
        public ActionResult NeuesItem(NewTodoItemDataModel entity)
        {
            var item = new TodoItem()
            {
                Name = entity.Name,
                TodoListeId = entity.TodoListeId
            };

            // Speichere das neue Item in der Datenbank
            _todoService.CreateNewItem(item);

            // Weiterleitung auf die Todo-Seite, mit der Liste des neu erzeugten Items als ausgewählte Liste
            return RedirectToAction("Index", "Todo", new { selectedListId = entity.TodoListeId });
        }

        [HttpGet]
        [Route("Todo/Item/Erledigt/{listeId}/{itemId}")]
        public ActionResult ItemErledigt(Int32 listeId, Int32 itemId)
        {
            // Setze das Item auf erledigt
            _todoService.ItemErledigt(itemId);

            // Weiterleitung auf die Todo-Seite, mit der Liste des erledigtem Items als ausgewählte Liste
            return RedirectToAction("Index", "Todo", new { selectedListId = listeId });
        }
    }
}