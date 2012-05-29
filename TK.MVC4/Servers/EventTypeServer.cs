using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK.Model.Servers;
using TK.MVC4.Models;

namespace TK.MVC4.Servers
{
    public class EventTypeServer
    {
        private EventTypeDataServer eventTypeDataServer = null;
        public EventTypeServer() {
            eventTypeDataServer = new EventTypeDataServer();
        }

        public List<EventTypeVM> GetAllEventTypes() {
            var eventTypes = eventTypeDataServer.GetEventTypes().ToList();
            return (from et in eventTypes
                    select EventTypeVM.FromDataObject(et)).ToList();
        }
    }
}