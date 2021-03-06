﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DatabaseObjects;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.AspNet.Identity;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Event
        public ActionResult GetEvent(int id)
        {
            //Create a Data holders
            var eventData = db.Events
                .Include(n => n.EventTags)
                .Include(n => n.EventUsers)
                .SingleOrDefault(n => n.Id == id);

            if (eventData == null)
                return HttpNotFound("Hittade ej event.");

            var eventOwnerProfileId =
                db.EventUsers.Where(x => x.IsOwner && x.EventId == id).Select(x => x.Profile.Id).FirstOrDefault();

            var ownerApplicationUserData = db.Users
                .SingleOrDefault(s => s.Profile.Id == eventOwnerProfileId);

            var ownerProfileData = db.Profiles
                .SingleOrDefault(n => n.Id == eventOwnerProfileId);



            //Creating a Model usning the Event Data holer
            var eventDetails = new EventDetailsViewModel()
            {
                Id = eventData.Id,
                Firstname = ownerProfileData.FirstName,
                LastName = ownerProfileData.LastName,
                PhoneNumber = ownerApplicationUserData.PhoneNumber,
                ImagePath = eventData.ImagePath,
                EventName = eventData.Name,
                Description = eventData.Description,
                StartDate = eventData.StartDate,
                EndDate = eventData.EndDate,
                Latitude = eventData.Latitude,
                Longitude = eventData.Longitude,
                MaxSignups = eventData.MaxSignups,
                MinSignups = eventData.MinSignups,
                Price = eventData.Price,

                Tags = db.EventTags
                                .Where(n => n.EventId == eventData.Id)
                                .Select(n => n.Tag)
                                .ToList()
            };


            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);

            var result = new GetEventViewModel()
            {
                Event = eventDetails,
                AlreadyBookedOnThisEvent = (loggedInUser == null) ? false : eventData.EventUsers.Any(n => n.EventId == eventData.Id && n.ProfileId == loggedInUser.Profile.Id),
                IsOwnerOfThisEvent = (loggedInUser == null) ? false : eventData.EventUsers.Any(n => n.EventId == eventData.Id && n.ProfileId == loggedInUser.Profile.Id && n.IsOwner),

                //DEBUG
                BookedUsers = eventData.EventUsers.Select(n => n.Profile).ToList(),
                //OwnerUsers = db.EventUsers.Where(n => n.EventId == eventData.Id && n.IsOwner).Select(n => n.Profile).ToList()
            };

            return View(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BookEvent(int eventId)
        {
            var loggedInUser = GetLoggedInStatus();

            if (loggedInUser == null)
                throw new Exception("Prova logga in på nytt!");

            var theBooking = new EventUser()
            {
                EventId = eventId,
                ProfileId = loggedInUser.Profile.Id,
                Status = "Confirmed",
                IsOwner = false,
            };

            var alreadyBooked =
                db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id);

            if (alreadyBooked)
                throw new Exception("Du är redan bokad på eventet.");

            db.EventUsers.Add(theBooking);
            db.SaveChanges();


            return PartialView("_BookingSystemPartial", new BookingSystemViewModel()
            {
                AlreadyBookedOnThisEvent = db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id),
                IsOwnerOfThisEvent = db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id && n.IsOwner),
                SpotsLeft = GetSpotsLeft(eventId),
                BookedUsers = db.EventUsers.Where(n => n.EventId == eventId && n.IsOwner == false).Select(n => n.Profile).ToList()
            });
        }

        [Authorize]
        public ActionResult UnBookEvent(int eventId ,string fromPage )
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);
            var loggedInUserProfile = db.Profiles.SingleOrDefault(n => n.Id == loggedInUser.Profile.Id);

            if (loggedInUser == null)
                throw new Exception("Du måste vara inloggad för att avboka en event");
            var findBokedEvent = db.EventUsers.SingleOrDefault(s => s.EventId == eventId && s.ProfileId == loggedInUserProfile.Id);
            var unBookEvent = db.EventUsers.Remove(findBokedEvent);
            db.EventUsers.Remove(unBookEvent);
            db.SaveChanges();
            if (fromPage == "GetEvent")
            {
                return PartialView("_BookingSystemPartial", new BookingSystemViewModel()
                {
                    AlreadyBookedOnThisEvent =
                        db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id),
                    IsOwnerOfThisEvent =
                        db.EventUsers.Any(
                            n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id && n.IsOwner),
                    SpotsLeft = GetSpotsLeft(eventId),
                    BookedUsers =
                        db.EventUsers.Where(n => n.EventId == eventId && n.IsOwner == false)
                            .Select(n => n.Profile)
                            .ToList()
                });
            }

            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        // GET: Events/Create
        [Authorize]
        public ActionResult Create()
        {
            var result = new CreateEventViewModel()
            {
                ExampleTags = db.Tags.OrderBy(n => Guid.NewGuid()).Take(4)
            };
            return View(result);
        }

        // POST: Events/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEventViewModel evm)
        {
            var loggedInUser = GetTheLoggedInUserAndCheckForNull();

            evm.ValidateInput(this);

            if (ModelState.IsValid)
            {
                // TODO: Use a default image if none is supplied by the user.

                // TODO: Make sure this path will be correct in the db!!
                var path = Path.Combine(Server.MapPath("/Content/EventImages/"), evm.Image.FileName);
                evm.Image.SaveAs(path);
                // TODO: Connect with path instead.
                string pathToSaveInDb = @"\Content\EventImages\" + evm.Image.FileName;


                // TODO: Refactor this code if there is time
                var tagsToAdd = evm.GenerateEventTags;

                AddNewTagsToDb(tagsToAdd);

                var result = new Event()
                {
                    Name = evm.Name,
                    Description = evm.Description,
                    StartDate = evm.StartDate,
                    EndDate = evm.EndDate,
                    MinSignups = evm.MinSignups,
                    MaxSignups = evm.MaxSignups,
                    Price = evm.Price,
                    Latitude = evm.Latitude,
                    Longitude = evm.Longitude,

                    ImagePath = pathToSaveInDb,
                };

                //Debug for just adding correct Tags to db.Tags
                db.Events.Add(result);
                db.SaveChanges();

                foreach (var tag in tagsToAdd)
                {
                    db.EventTags.Add(new EventTag()
                    {
                        Tag = db.Tags.SingleOrDefault(n => n.Name == tag.Name),
                        EventId = result.Id
                    });
                }


                var eventOwner = new EventUser()
                {
                    Event = result,
                    ProfileId = loggedInUser.Profile.Id,
                    Status = "Confirmed",
                    IsOwner = true,
                };
                db.EventUsers.Add(eventOwner);
                db.SaveChanges();

                return RedirectToAction("GetEvent", new { id = result.Id });
            }

            evm.ExampleTags = db.Tags.OrderBy(n => Guid.NewGuid()).Take(4);
            return View(evm);
        }

        public ActionResult GetBookingData(int eventId)
        {
            var loggedInUser = GetLoggedInStatus();

            return PartialView("_BookingSystemPartial", new BookingSystemViewModel()
            {
                AlreadyBookedOnThisEvent = (loggedInUser == null) ? false : db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id),
                IsOwnerOfThisEvent = (loggedInUser == null) ? false : db.EventUsers.Any(n => n.EventId == eventId && n.ProfileId == loggedInUser.Profile.Id && n.IsOwner),
                SpotsLeft = GetSpotsLeft(eventId),
                BookedUsers = db.EventUsers.Where(n => n.EventId == eventId && n.IsOwner == false).Select(n => n.Profile).ToList()
            });
        }


        private ApplicationUser GetTheLoggedInUserAndCheckForNull()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);

            if (loggedInUser == null)
                throw new Exception("Du måste vara inloggad för denna funktion!");
            return loggedInUser;
        }

        private ApplicationUser GetLoggedInStatus()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);

            return loggedInUser;
        }

        private void AddNewTagsToDb(List<Tag> tagsToAdd)
        {
            //tagsToAdd is the incoming shit, with all the tags to add to EventTags in DB
            // but the list needs to be filtered for any existing tags in db.Tags!!
            var tagsToAddToDb = new List<Tag>(tagsToAdd);
            var currentTags = db.Tags.ToList();


            var result = new List<Tag>();
            foreach (var tag in tagsToAddToDb.Where(n => currentTags.All(n2 => n2.Name != n.Name)))
            {
                result.Add(tag);
            }

            db.Tags.AddRange(result);


            // NOTICE! Have to savechanges later!
        }

        private int? GetSpotsLeft(int id)
        {
            //var result1 = db.EventUsers.Local.Count(s => s.EventId == id);
            var result1 = db.EventUsers.Count(s => s.EventId == id);
            var maxSignups = db.Events.Find(id).MaxSignups + 1;
            if (maxSignups == null)
                return null;

            var result = maxSignups.Value - result1;
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}