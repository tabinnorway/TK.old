using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TK.MVC4.Models;
using System.Text;
using TK.Model.Servers;

namespace TK.MVC4.Controllers
{
    public class UtilController : Controller
    {
        private EventDataServer evtServer = null;
        private LocationDataServer locationServer = null;
        private MemberDataServer memberServer = null;
        private ScoreServer scoreServer = null;
        public UtilController() {
            evtServer = new EventDataServer();
            locationServer = new LocationDataServer();
            memberServer = new MemberDataServer();
            scoreServer = new ScoreServer();
        }

        //
        // GET: /Util/
        [Authorize(Roles="Administrator,Power User")] 
        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Power User")]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files) {
            int[] sequence = { 3, 1, 0, 2 };

            List<string> successes = new List<string>();
            List<string> failures = new List<string>();

            var fileList = files.ToList();
            foreach (int fileNo in sequence) {
                HttpPostedFileBase file = null;
                try {
                    file = fileList[fileNo];
                }
                catch {
                }

                if (file != null && file.ContentLength > 0) {
                    Encoding inEnc = Encoding.GetEncoding("IBM865");
                    StreamReader sr = new StreamReader(file.InputStream, inEnc);

                    string line = null;
                    bool isFirstLine = true;
                    Dictionary<string, int> newLines = new Dictionary<string, int>();
                    while ((line = sr.ReadLine()) != null) {
                        if (isFirstLine) { isFirstLine = false; continue; }

                        string error = null;
                        switch (fileNo) {
                            case 0:
                                makeEvent(line, out error);
                                if (error != null) {
                                    failures.Add(string.Format(@"Feil ""{0}"" ved opprettelsen av arrangement fra ""{1}""", error, line));
                                    error = null;
                                }
                                else {
                                    successes.Add(string.Format(@"La til arrangement fra: ""{0}""", line));
                                }
                                break;
                            case 1:
                                makeMember(line, out error);
                                if (error != null) {
                                    failures.Add(string.Format(@"Feil {0} ved opprettelsen av medlem fra ""{1}""", error, line));
                                    error = null;
                                }
                                else {
                                    successes.Add(string.Format(@"La til medlem fra: ""{0}""", line));
                                }
                                break;
                            case 2:
                                makeScore(line, out error);
                                if (error != null) {
                                    failures.Add(string.Format(@"Feil ""{0}"" ved lagring av terningkast fra ""{1}""", error, line));
                                    error = null;
                                }
                                else {
                                    successes.Add(string.Format(@"La til terningkast fra: ""{0}""", line));
                                }
                                break;
                            case 3:
                                makeLocation(line, out error);
                                if (error != null) {
                                    failures.Add(string.Format(@"Feil ""{0}"" ved opprettelsen av lokasjon fra ""{1}""", error, line));
                                    error = null;
                                }
                                else {
                                    successes.Add(string.Format(@"La til lokasjon fra: ""{0}""", line));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            ViewBag.successes = successes;
            ViewBag.failures = failures;
            return View( "FinishedUploading" );
        }

        private void makeScore(string line, out string error) {
            error = null;
            MemberEventScoreVM memberScore = MemberEventScoreVM.FromCSVLine(line);
            var scoreAdded = scoreServer.AddMemberEventScore(memberScore.ToDataObject(), out error);
            if (scoreAdded == null || error != null) {
                Console.WriteLine(error);
            }
        }

        private void makeMember(string line, out string error) {
            error = null;
            try {
                MemberVM member = MemberVM.FromCSVLine(line);
                var memberadded = memberServer.AddMember(member.ToMember(), out error);
            }
            catch (Exception x) {
                error = x.Message;
            }
        }

        private void makeLocation(string line, out string error) {
            error = null;
            try {
                LocationVM loc = LocationVM.FromCSVLine(line);
                var locAdded = locationServer.AddLocation(loc.ToDataObject(), out error);
            }
            catch (Exception x) {
                error = x.Message;
            }
        }

        private void makeEvent(string line, out string error) {
            error = null;
            try {
                EventVM evt = EventVM.FromCSVLine(line);
                var evtAdded = evtServer.AddEvent(evt.ToEvent(), out error);
            }
            catch (Exception x) {
                error = x.Message;
            }
        }

        private string decode(string line, Encoding inEnc, Encoding outEnc) {
            byte[] inBytes = inEnc.GetBytes(line);
            byte[] outBytes = Encoding.Convert(inEnc, outEnc, inBytes);

            String outString = outEnc.GetString(outBytes);
            return outString;
        }

        //
        // GET: /Util/Details/5
        public ActionResult Details(int id) {
            return View();
        }

        //
        // GET: /Util/Create
        public ActionResult Create() {
            return View();
        }

        //
        // POST: /Util/Create
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
        // GET: /Util/Edit/5
        public ActionResult Edit(int id) {
            return View();
        }

        //
        // POST: /Util/Edit/5
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
        // GET: /Util/Delete/5
        public ActionResult Delete(int id) {
            return View();
        }

        //
        // POST: /Util/Delete/5
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
