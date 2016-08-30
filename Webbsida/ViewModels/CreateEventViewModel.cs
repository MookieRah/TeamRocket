using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }

        //public virtual List<EventUser> EventUsers { get; set; }

        [StringLength(25, ErrorMessage = "Eventnamnet måste vara mellan 2 och 25 tecken"), MinLength(2, ErrorMessage = "Eventnamnet måste vara mellan 2 och 25 tecken")]
        [Required(ErrorMessage = "Eventnamn krävs")]
        [Display(Name = "Eventnamn")]
        public string Name { get; set; }

        [StringLength(600, ErrorMessage = "Beskrivningen måste vara mellan 25 och 600 tecken"), MinLength(25, ErrorMessage = "Beskrivningen måste vara mellan 25 och 600 tecken")]
        [Required(ErrorMessage = "Beskrivning krävs")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Du måste sätta ett startdatum")]
        [Display(Name = "Startdatum")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Du måste sätta ett slutdatum")]
        [Display(Name = "Slutdatum")]
        public DateTime EndDate { get; set; }
        
        [Required(ErrorMessage = "Vänligen välj vart eventet ska hållas.")]
        public float Latitude { get; set; }
        [Required(ErrorMessage = " ")]
        public float Longitude { get; set; }

        [Display(Name = "Max antal deltagare")]
        public int? MaxSignups { get; set; }
        [Display(Name = "Minsta antal deltagare")]
        public int? MinSignups { get; set; }

        [Display(Name = "Eventuell kostnad")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Vänligen välj en bild för eventet")]
        [DataType(DataType.Upload)]
        [Display(Name = "Bild")]
        public HttpPostedFileBase Image { get; set; }
    }
}
