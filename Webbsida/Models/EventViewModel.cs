using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webbsida.Models
{
    public class EventViewModel
    {

        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxSignups { get; set; }
        public int? MinSignups { get; set; }
        public decimal? Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int PhoneNumber { get; set; }
        

    }
}