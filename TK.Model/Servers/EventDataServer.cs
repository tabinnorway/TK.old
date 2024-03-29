﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace TK.Model.Servers
{
    public class EventDataServer
    {
        private TKEntities entities = null;
        public EventDataServer() {
            entities = new TKEntities();
        }

        public IQueryable<Event> GetAllIncludeScores() {
            return entities.Events.Include("MemberEventScores").Include("EventType").Include("Location").OrderByDescending(e => e.Date);
        }
        public bool HasEvent(Event evt) {
            try {
                return entities.Events.Where(e => e.LocationId == evt.LocationId && e.Date == evt.Date).Count() > 0;
            }
            catch {
            }
            return false;
        }
        public Event AddEvent( Event evt, out string error ) {
            error = null;
            Event retval = null;
            if (evt.Id > 0 || HasEvent(evt)) {
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
