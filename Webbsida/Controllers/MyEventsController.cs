using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Webbsida.Models;
using Webbsida.Controllers;
using Microsoft.AspNet.Identity;


namespace Webbsida.Controllers
{
    public class MyEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationDbContext db1 = new ApplicationDbContext();
        private ApplicationDbContext db2 = new ApplicationDbContext();
        private ApplicationDbContext db3 = new ApplicationDbContext();
        public ActionResult Index()
        {
            //my events
            List<object> eventdetails = new List<object> { };
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);
            var myevents = db.EventUsers.Where(x => x.ProfileId == loggedInUser.Profile.Id);

            var results = new EventViewModel();

          if (myevents != null)
          {
                foreach (var myevent  in myevents )
            {

                    results.Events.Add(db1.Events.SingleOrDefault(x => x.Id == myevent.EventId));
                
            }

          }
            // my booked events

            List<object> bookedeventdetails = new List<object> { };
            var loggedUserId = User.Identity.GetUserId();
            var loggedUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);
            var mybookedevents = db.EventUsers.Where(x => x.ProfileId == loggedInUser.Profile.Id && x.IsOwner == false);

          if (mybookedevents != null)
          {
            foreach (var mybookedevent in mybookedevents)
            {
                results.BookedEvents.Add(db2.Events.SingleOrDefault(x => x.Id == mybookedevent.EventId));
            }
          }



            //Account settings 
            if (loggedInUser.Profile.Id != null)
            {
                results.Profiles.Equals(db3.Profiles.Where(x => x.Id == loggedInUser.Profile.Id).SingleOrDefault());
                var y =db3.Profiles.Where(x => x.Id == loggedInUser.Profile.Id).SingleOrDefault();

                results.Profiles.FirstName = y.FirstName;
                results.Profiles.LastName = y.LastName;
                //results.Profiles.LastName.Equals(db3.Profiles.Where(x => x.Id == loggedInUser.Profile.Id).SingleOrDefault());
                //var z = db3.Users.Where(x => x.Id == loggedUserId.ToString()).SingleOrDefault();

                //results.ApplicationUsers.Email= z.Email;
                //results.ApplicationUsers.PhoneNumber = z.PhoneNumber;
                //results.ApplicationUsers.Email.Equals(db3.Users.Where(x => Int32.Parse(x.Id) == loggedInUser.Profile.Id).SingleOrDefault());
            }

          return View(results);
            
        }

       
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db1.Dispose();
                db2.Dispose();
                db3.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
