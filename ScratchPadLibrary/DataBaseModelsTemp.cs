using System;
using System.Collections.Generic;

namespace ScratchPadLibrary
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }    //Or just email?
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthYear { get; set; }
        public UserType UserType { get; set; }
        //public string HomeTown { get; set; }
        // + LocationLatitude & LocationLongitude = UserPosition

        // + Monitored keywords/EventTags?

        public virtual UserSearch UserSearches { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public int? MaxSignups { get; set; }
        public decimal? Cost { get; set; }

        public virtual User Owner { get; set; }
        public virtual EventCategory EventCategory { get; set; }
        public virtual List<EventTag> EventTags { get; set; }
        public virtual List<EventSignup> EventSignups { get; set; }
    }

    public class EventSignup
    {
        public int Id { get; set; }
        public DateTime SignupDate { get; set; }
        public SignupStatus SignupStatus { get; set; }

        public virtual User User { get; set; }
    }

    public class EventTag
    {
        public int Id { get; set; }
        public string TagName { get; set; }

        public virtual Event Event { get; set; }
    }

    public class EventCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual Event Event { get; set; }
    }

    public class UserSearch
    {
        public string SearchString { get; set; }

        public virtual User User { get; set; }
    }

    public enum UserType
    {
        Private,
        Commercial
    }

    public enum SignupStatus
    {
        Pending,
        Confirmed
    }
}
