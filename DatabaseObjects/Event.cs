using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DatabaseObjects
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public virtual List<EventUser> EventUsers  { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int? MaxSignups { get; set; }
        public int? MinSignups { get; set; }

        public decimal? Price { get; set; }

        public Profile GetOwner()
        {
            return EventUsers.Where(x => x.IsOwner == true).Select(x => x.Profile).FirstOrDefault();
        }

        public string ImagePath { get; set; }
    }
}
