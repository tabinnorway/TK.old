using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TK.MVC4.Servers;
using TK.MVC4.Models;

namespace TK.MVC4.Controllers
{
    public class MembersController : Controller
    {
        private MemberServer membersServer = null;
        public MembersController() {
            membersServer = new MemberServer();
        }

        //
        // GET: /Members/
        public ActionResult Index() {
            return View( membersServer.GetMembers() );
        }

        //
        // GET: /Members/Details/5
        public ActionResult Details(long id) {
            return View(membersServer.GetMemberAndScores(id));
        }

        //
        // GET: /Members/Create
        public ActionResult Create() {
            return View();
        }

        //
        // POST: /Members/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection) {
            try {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Members/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Members/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }

        //
        // GET: /Members/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        //
        // POST: /Members/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch {
                return View();
            }
        }
    }
}
