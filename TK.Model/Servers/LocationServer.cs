using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class LocationDataServer
    {
        private TKEntities entities = null;
        private static Dictionary<string, Location> locationsByName = null;
        public LocationDataServer() {
            entities = new TKEntities();
            if (locationsByName == null) {
                locationsByName = new Dictionary<string, Location>();
                lock (locationsByName) {
                    foreach (var loc in entities.Locations) {
                        locationsByName.Add(loc.Name, loc);
                    }
                }
            }
        }
        public bool HasLocation(Location loc) {
            try {
                return entities.Locations.Where(l => l.Name == loc.Name).Count() > 0;
            }
            catch {
            }
            return false;
        }
        public Location AddLocation(Location loc, out string error) {
            error = null;
            Location retval = null;
            if (loc.Id > 0 || HasLocation(loc) ) {
                error = "Can not add a location that has already been added";
            }
            else {
                try {
                    loc.CreatedAt = DateTime.Now;
                    loc.LastUpdated = DateTime.Now;
                    entities.AddToLocations(loc);
                    entities.SaveChanges();
                    retval = loc;
                }
                catch (Exception x) {
                    error = x.Message + (x.InnerException != null ? "Inner exception: " + x.InnerException.Message : "");
                    retval = null;
                }
            }
            return retval;
        }
        public long GetLocationIdByName( string name ) {
            if (locationsByName.ContainsKey(name)) {
                return locationsByName[name].Id;
            }

            try {
                return entities.Locations.Where(loc => loc.Name.Equals(name, StringComparison.CurrentCulture)).SingleOrDefault().Id;
            }
            catch {
            }
            return -1;
        }

        public IQueryable<Location> GetAll() {
            return entities.Locations.OrderBy(loc => loc.Name);
        }
    }
}
