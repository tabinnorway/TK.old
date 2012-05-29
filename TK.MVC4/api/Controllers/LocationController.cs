using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TK.MVC4.Models;
using TK.MVC4.Servers;

namespace TK.MVC4.api.Controllers.Controllers
{
    public class LocationController : ApiController
    {
        private LocationServer locationServer = null;
        private LocationController() {
            locationServer = new LocationServer();
        }

        // GET /api/<controller>
        public IEnumerable<LocationVM> Get() {
            return locationServer.GetAll();
        }

        // GET /api/<controller>/5
        public string Get(int id) {
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