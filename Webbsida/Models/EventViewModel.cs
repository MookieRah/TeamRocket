using DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Webbsida.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        public virtual List<EventUser> EventUsers { get; set; }

        [DisplayName("Eventname")]
        [StringLength(25, ErrorMessage = "The eventnamne can olny be between 8 and 25 character"), MinLength(8, ErrorMessage = "The eventnamne can olny be between 8 and 50 character")]
        public string Name { get; set; }

        [DisplayName("Description")]
        [StringLength(600, ErrorMessage = "The description can olny be between 25 and 600 character"), MinLength(25, ErrorMessage = "The description can olny be between 25 and 600 character")]
        public string Description { get; set; }

        [DisplayName("Startdate")]
        //[StringLength(600, ErrorMessage = "The description can olny be between 25 and 600 character"), MinLength(25, ErrorMessage = "The description can olny be between 25 and 600 character")]
        public DateTime StartDate { get; set; }

        [DisplayName("Enddate")]
        public DateTime EndDate { get; set; }


        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [DisplayName("Max visitor")]
        public int? MaxSignups { get; set; }

        [DisplayName("Less visitor")]
        public int? MinSignups { get; set; }

        [DisplayName("Price")]
        public decimal? Price { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }
    }
}