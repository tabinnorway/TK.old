using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Model.Servers
{
    public class LocationServer
    {
        private TKEntities entities = null;
        private static Dictionary<string, Location> locationsByName = null;
        public LocationServer() {
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
        public Location AddLocation(Location loc, out string error) {
            error = null;
            Location retval = null;
            if (loc.Id > 0) {
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
            string theVal = "ÅL STORESALEN";
            if (name == theVal) {
                Console.WriteLine("At Ål");
                foreach( string key in locationsByName.Keys ) {
                    if (key.Contains("STORE")) {
                        string comparer = "'" + name + "' compares to\n'" + key + "' gives";
                        int res = string.Compare(key, name);
                        comparer += res.ToString();
                    }
                }
            }
            if (locationsByName.ContainsKey(name)) {
                return locationsByName[name].Id;
            }
            if (theVal.Equals(name)) {
                Console.WriteLine("They seem to be equal");
            }
            try {
                return entities.Locations.Where(loc => loc.Name.Equals(name, StringComparison.CurrentCulture)).SingleOrDefault().Id;
            }
            catch {
            }
            return -1;
        }
    }
}
