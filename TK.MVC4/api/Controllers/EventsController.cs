using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TK.MVC4.Servers;
using TK.MVC4.Models;

namespace TK.MVC4.api.Controllers
{
    public class EventsController : ApiController
    {
        private EventServer eventServer = null;

        public EventsController() {
            eventServer = new EventServer();
        }

        // GET /api/<controller>
        public IEnumerable<EventVM> Get() {
            return eventServer.GetAllIncludeScores();
        }

        // GET /api/<controller>/5
        public string Get(long id) {
            return "value";
        }

        // POST /api/<controller>
        public void Post(string value) {
        }

        // PUT /api/<controller>/5
        public void Put(int id, string value) {
        }

        // DELETE /api/<controller>/5
        public void Delete(int id) {
        }
    }
}