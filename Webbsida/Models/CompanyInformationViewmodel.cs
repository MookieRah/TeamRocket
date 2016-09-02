using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Webbsida.Models
{
    public class CompanyInformationViewmodel
    {
        [Key]
        public int Id { get; set; }
    
        [Display(Name = "Adress: ")]
        public string Address{ get; set; }  

        [Display(Name = "Beskrivning: ")]
        public string Description { get; set; }

        [Display(Name = "Longitud: ")]
        public float Longitude { get; set; }

        [Display(Name = "Latitud: ")]
        public float Altitude { get; set; }

        [Display(Name = "Telefon nummer: ")]
        public string Phonenumber { get;  set; }
    }
}