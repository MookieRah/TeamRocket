using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class EventDetailsViewModel
    {
        //TODO: Display(Name = "") på alla
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string EventName { get; set; }
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "StartDatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Slutdatum")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Max antal deltagare")]
        public int? MaxSignups { get; set; }

        [Display(Name = "Minsta antal deltagare")]
        public int? MinSignups { get; set; }

        [Display(Name = "Eventuell kostnad")]
        public decimal? Price { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [Display(Name="Telefon")]
        public string PhoneNumber { get; set; }

        public string ImagePath { get; set; }
        //TODO Delete ID for asome Idea
        public int Id { get; set; }

        public List<Tag> Tags { get; set; }

        [Display(Name = "Skapare")]
        public string Name => Firstname + " " + LastName;
    }
}