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
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public string Adress { get; set; }

        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public string PhoneNumber { get; set; } 
    }
}
