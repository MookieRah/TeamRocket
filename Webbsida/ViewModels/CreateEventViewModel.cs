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

        [StringLength(25, ErrorMessage = "The eventnamne can olny be between 8 and 25 character"), MinLength(8, ErrorMessage = "The eventnamne can olny be between 8 and 50 character")]
        public string Name { get; set; }

        [StringLength(600, ErrorMessage = "The description can olny be between 25 and 600 character"), MinLength(25, ErrorMessage = "The description can olny be between 25 and 600 character")]
        public string Description { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "required")]
        public DateTime EndDate { get; set; }


        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public int? MaxSignups { get; set; }
        public int? MinSignups { get; set; }

        public decimal? Price { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }
    }
}
