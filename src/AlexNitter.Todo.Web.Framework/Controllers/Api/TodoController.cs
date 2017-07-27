using AlexNitter.Todo.Lib.Services;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace AlexNitter.Todo.WebFramework.Controllers.Api
{
    // Das Authorie-Attribut zeigt an, dass diese Routen nur von autorisierten Usern aufgerufen werden dürfen.
    // Autorisiert bedeutet in diesen Kontext, dass die Eigenschaft HttpContext.User gesetzt ist
    [Authorize]
    public class TodoController : ApiController
    {
        private TodoService _todoService = new TodoService();

        [HttpGet]
        [Route("Api/Todo/Listen")]
        public IHttpActionResult GetListen()
        {
            // Hole alle Listen des authentifizierten Users
            var listen = _todoService.GetListenFromUser(HttpContext.Current.User.Identity.Name);

            // Gib sie als JSON oder XML zurück, je nach "Accept-ContentType"-Header im HTTP-Response
            return Ok(listen);
        }

        [HttpGet]
        [Route("Api/Todo/Items/{listeId}")]
        public IHttpActionResult GetItemsByListe(Int32 listeId)
        {
            // Prüfe, ob der authentifizierte User die Liste abfragen darf und hole Items zur Liste
            var listen = _todoService.GetListenFromUser(HttpContext.Current.User.Identity.Name);
            var liste = listen.FirstOrDefault(x => x.Id == listeId);

            if (liste != null && liste.Items != null && liste.Items.Count > 0)
            {
                // Wenn es Items zur Liste gibt, gib sie als JSON oder XML zurück, je nach "Accept-ContentType"-Header im HTTP-Response
                return Ok(liste.Items);
            }
            else
            {
                // Wenn es keine Items zur Liste gibt, gib nichts zurück
                return Ok();
            }
        }
    }
}