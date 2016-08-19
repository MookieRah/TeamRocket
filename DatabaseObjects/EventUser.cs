using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace DatabaseObjects
{
    public class EventUser
    {
        [Key]
        public int Id { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string Status { get; set; }

        public bool IsOwner { get; set; }
    }
}
