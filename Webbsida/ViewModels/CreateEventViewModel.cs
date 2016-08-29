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

        public virtual List<EventUser> EventUsers { get; set; }

        [Display(Name ="Namn")]
        [StringLength(25, ErrorMessage = "Eventnamnet kan endast vara mellan 8 och 25 tecken"), MinLength(8, ErrorMessage = "Eventnamnet kan endast vara mellan 8 och 25 tecken")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        [StringLength(600, ErrorMessage = "Beskrivningen kan bara vara mellan 25 och 600 tecken"), MinLength(25, ErrorMessage = "Beskrivningen kan bara vara mellan 25 och 600 tecken")]
        public string Description { get; set; }

        [Display(Name = "Starttid")]
        [Required(ErrorMessage = "required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Sluttid")]
        [Required(ErrorMessage = "required")]
        public DateTime EndDate { get; set; }


        public float Latitude { get; set; }
        public float Longitude { get; set; }


        [Display(Name = "Max antal")]
        public int? MaxSignups { get; set; }

        [Display(Name = "Minst antal")]
        public int? MinSignups { get; set; }


        [Display(Name = "Eventkostnad")]
        public decimal? Price { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }
    }
}
