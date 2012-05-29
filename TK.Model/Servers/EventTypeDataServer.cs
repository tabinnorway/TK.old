using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class EventTypeDataServer
    {
        private TKEntities entities = null;
        public EventTypeDataServer() {
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

        public IQueryable<EventType> GetEventTypes() {
            return entities.EventTypes.OrderBy(et => et.Name);
        }
    }
}
