using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseObjects
{
    public class CompanyInformation
    {
        public CompanyInformation()
        {
            Address = "Tvistevägan 48";
            Description = "Vi är ett företag som har till mål att ensamma pesoner ska hitta aktiviteter för att göra med andra människpr";
            Phonenumber = "090-693534";
        }
        [Key]
        public int Id { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; } 

        [Display(Name = "Beskrivning")]
        public string Description { get; set; } 

        [Display(Name = "Longitud")]
        public double Longitude { get; set; }

        [Display(Name = "Latitud")]
        public double Latitude { get; set; }

        [Display(Name = "Telefon nummer")]
        public string Phonenumber { get; set; } 
    }
}
