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
               var y = db.Profiles.SingleOrDefault(x => x.Id == loggedInUser.Profile.Id);

                var temp = new Profile()
                {
                    FirstName = y.FirstName,
                    LastName =  y.LastName
                };

                results.Profiles = temp;


                var z = db.Users.SingleOrDefault(x => x.Id == loggedUserId.ToString());
                var temp2 = new ApplicationUser()
                {
                    Email = z.Email,
                    PhoneNumber = z.PhoneNumber,
                };
                results.ApplicationUsers = temp2;
                
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

        //public ActionResult Index()
        //{
        //    return View(db.Events.ToList());
        //}

        // GET: test/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: test/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: test/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: test/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: test/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: test/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }



}
