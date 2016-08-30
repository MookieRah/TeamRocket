using System;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.ViewModels
{
    public class GetEventViewModel
    {
        public EventDetailsViewModel Event { get; set; }
        public ApplicationUser LoggedInUser { get; set; }
    }

    public class EventDetailsViewModel
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
        public string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        //TODO Delete ID for asome Idea
        public int Id { get; set; }
    }
}

