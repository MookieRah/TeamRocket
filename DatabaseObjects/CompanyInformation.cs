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

        public string Description { get; set; }
        public float Longitude { get; set; }
        public float Altitude { get; set; }
        public string Phonenumber { get; set; }


    }
}
