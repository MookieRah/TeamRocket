using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseObjects
{
    class CompanyInformation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Beskrivning" )]
        public string Descripption { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Longitude")]
        public float Longitude { get; set; }

        [Display(Name = "Altitude")]
        public float Altitude { get; set; }

        [Display(Name = "Telefon nummer")]
        public int PhoneNumber { get; set; }

    }
}
