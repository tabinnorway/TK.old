using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TK.Model.Servers;
using TK.MVC4.Models;

namespace TK.MVC4.Servers
{
    public class LocationServer
    {
        private LocationDataServer locationDataServer = null;
        public LocationServer() {
            locationDataServer = new LocationDataServer();
        }

        public List<LocationVM> GetAll() {
            var locations = locationDataServer.GetAll().ToList();
            return (from evt in locations
                    select LocationVM.FromDataObject(evt)).ToList();
        }
    }
}