using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class IndexEventViewModel
    {
        public IndexEventViewModel(Event eventDbo)
        {
            this.Id = eventDbo.Id;
            this.EventUsers = eventDbo.EventUsers;
            this.Name = eventDbo.Name;
            this.Description = eventDbo.Description;
            this.StartDate = eventDbo.StartDate;
            this.EndDate = eventDbo.EndDate;
            this.Latitude = eventDbo.Latitude;
            this.Longitude = eventDbo.Longitude;
            this.MaxSignups = eventDbo.MaxSignups;
            this.MaxSignups = eventDbo.MinSignups;
            this.Price = eventDbo.Price;
            this.ImagePath = eventDbo.ImagePath;
        }

        public int Id { get; set; }

        public List<EventUser> EventUsers { get; set; }

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

        public string GetPartialDescription(int num)
        {
            return new string(Description.ToCharArray(0, num)) + "...";
        }
    }
}