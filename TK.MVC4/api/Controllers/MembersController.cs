using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using TK.MVC4.Models;
using TK.MVC4.Servers;

namespace TK.MVC4.api.Controllers
{
    public class MemberController : ApiController
    {
        private MemberServer membersServer = null;
        public MemberController() {
            membersServer = new MemberServer();
        }
        // GET /api/<controller>
        public IEnumerable<MemberVM> Get() {
            return membersServer.GetMembers();
        }

        // GET /api/<controller>/5
        public MemberVM Get(long id) {
            return membersServer.GetMember(id);
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