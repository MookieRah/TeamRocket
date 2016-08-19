using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DatabaseObjects
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        public virtual IEnumerable<EventUser> EventUsers { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public bool IsPrivate { get; set; }



        //public virtual List<Event> Events { get; set; }
    }
}
