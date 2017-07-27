using AlexNitter.Todo.Lib.DataModels;
using AlexNitter.Todo.Lib.Entities;
using AlexNitter.Todo.Lib.Services;
using AlexNitter.Todo.Lib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AlexNitter.Todo.Web.Core.Controllers
{
    // Das Authorie-Attribut zeigt an, dass diese Routen nur von autorisierten Usern aufgerufen werden dürfen.
    // Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
    [Authorize]
    public class TodoController : Controller
    {
        private TodoService _todoService = new TodoService();

        #region API-Routen für die REST-Schnittstelle
        [HttpGet]
        [Route("Api/Todo/Listen")]
        public IActionResult GetListen()
        {
            // Hole alle Listen des authentifizierten Users
            var listen = _todoService.GetListenFromUser(HttpContext.User.Identity.Name);

            // Gib sie als JSON oder XML zurück, je nach "Accept-ContentType"-Header im HTTP-Response
            return new ObjectResult(listen);
        }

        [HttpGet]
        [Route("Api/Todo/Items/{listeId}")]
        public IActionResult GetItemsByListe(Int32 listeId)
        {
            // Prüfe, ob der authentifizierte User die Liste abfragen darf und hole Items zur Liste
            var listen = _todoService.GetListenFromUser(HttpContext.User.Identity.Name);
            var liste = listen.FirstOrDefault(x => x.Id == listeId);

            if (liste != null && liste.Items != null && liste.Items.Count > 0)
            {
                // Wenn es Items zur Liste gibt, gib sie als JSON oder XML zurück, je nach "Accept-ContentType"-Header im HTTP-Response
                return new ObjectResult(liste.Items);
            }
            else
            {
                // Wenn es keine Items zur Liste gibt, gib nichts zurück
                return new ObjectResult(null);
            }
        }
        #endregion


        #region MVC-Routen für die HTML-Schnittstelle
        [HttpGet]
        [Route("Todo/{*selectedListId}")]
        public IActionResult Index(Nullable<Int32> selectedListId)
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
        public IActionResult NeueListe(TodoListe entity)
        {
            // Speichere die übergebene Liste in der Datenbank
            _todoService.CreateNewListe(entity, HttpContext.User.Identity.Name);

            // Weiterleitung auf die Todo-Seite, mit der neu erzeugten Liste als ausgewählte Liste
            return RedirectToAction("Index", "Todo", new { selectedListId = entity.Id });
        }
        
        [HttpGet]
        [Route("Todo/Liste/Delete/{listeId}")]
        public IActionResult ListeLoeschen(Int32 listeId)
        {
            // Lösche die übergebene Liste aus der Datenbank
            _todoService.DeleteListe(listeId);

            // Weiterleitung auf die Todo-Seite, ohne Angabe zu einer ausgewählten Liste
            return RedirectToAction("Index", "Todo");
        }

        [HttpPost]
        [Route("Todo/Item/Neu")]
        public IActionResult NeuesItem(NewTodoItemDataModel entity)
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
        public IActionResult ItemErledigt(Int32 listeId, Int32 itemId)
        {
            // Setze das Item auf erledigt
            _todoService.ItemErledigt(itemId);

            // Weiterleitung auf die Todo-Seite, mit der Liste des erledigtem Items als ausgewählte Liste
            return RedirectToAction("Index", "Todo", new { selectedListId = listeId });
        }
        #endregion
    }
}
