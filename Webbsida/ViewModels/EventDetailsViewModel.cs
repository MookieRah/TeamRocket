using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class EventDetailsViewModel
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
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

        public List<Tag> Tags { get; set; }
    }
}