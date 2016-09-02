using DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webbsida.Models
{
    public class EventViewModel
    {
        //List<Event> Events { get; set; }
        public List<Event> Events = new List<Event>();
        public List<Event> BookedEvents = new List<Event>();
        public Profile Profiles = new Profile();
        public ApplicationUser ApplicationUsers = new ApplicationUser();

    }
}