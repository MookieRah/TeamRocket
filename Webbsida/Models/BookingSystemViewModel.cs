using System.Collections.Generic;
using DatabaseObjects;

namespace Webbsida.ViewModels
{
    public class BookingSystemViewModel
    {
        public bool IsOwnerOfThisEvent { get; set; }
        public bool AlreadyBookedOnThisEvent { get; set; }

        public int? SpotsLeft { get; set; }
        public IEnumerable<Profile> BookedUsers { get; set; }
    }
}