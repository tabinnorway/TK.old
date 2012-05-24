using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class EventDataServer
    {
        private TKEntities entities = null;
        public EventDataServer() {
            entities = new TKEntities();
        }

        public Event AddEvent( Event evt, out string error ) {
            error = null;
            Event retval = null;
            if (evt.Id > 0) {
                error = "Can not add an event that has already been added";
            }
            else {
                try {
                    entities.AddToEvents(evt);
                    entities.SaveChanges();
                    retval = evt;
                }
                catch (Exception x) {
                    error = x.Message + (x.InnerException != null ? "Inner exception: " + x.InnerException.Message : "");
                    retval = null;
                }
            }
            return retval;
        }

        public Event GetEventByDate(DateTime when) {
            return entities.Events.Where(e => e.Date == when).SingleOrDefault();
        }

        public Event GetEvent(long id) {
            try {
                return entities.Events.SingleOrDefault(e => e.Id == id);
            }
            catch {
            }
            return null;
        }
    }
}
