using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK.Model.Servers;
using TK.MVC4.Models;

namespace TK.MVC4.Servers
{
    public class EventServer
    {
        private EventDataServer eventDataServer = null;
        public EventServer() {
            eventDataServer = new EventDataServer();
        }

        public EventVM Add(EventVM evt, out string error) {
            return EventVM.FromEvent(eventDataServer.AddEvent(evt.ToEvent(), out error));
        }
    }
}