using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class EventTypeServer
    {
        private TKEntities entities = null;
        public EventTypeServer() {
            entities = new TKEntities();
        }
        public long GetEventTypeIdByName(string name) {
            try {
                return entities.EventTypes.Where(et => et.Name == name).SingleOrDefault().Id;
            }
            catch {
            }
            return -1;
        }
    }
}
