using System.Collections.Generic;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.ViewModels
{
    public class GetEventViewModel
    {
        public EventDetailsViewModel Event { get; set; }
        public ApplicationUser LoggedInUser { get; set; }
        public bool AlreadyBookedOnThisEvent { get; set; }
        public bool IsOwnerOfThisEvent { get; set; }

        public List<Profile> BookedUsers { get; set; }
        public List<Profile> OwnerUsers { get; set; }
    }
}

