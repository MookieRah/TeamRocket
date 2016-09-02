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
            var eventData = db.Events.Find(id);

            var lePhone = db.Users
                .Where(s => s.Profile.Id == id)
                .Select(p => p.PhoneNumber).SingleOrDefault();

            var eventUserData = db.EventUsers
                .Where(d => d.EventId == id)
                .Select(g => g.EventId).FirstOrDefault();

            var userDataFirstName =
                db.Profiles.Where(d => d.Id == eventUserData).Select(f => f.FirstName).SingleOrDefault();
            var userDataLastName =
                db.Profiles.Where(d => d.Id == eventUserData).Select(f => f.LastName).SingleOrDefault();



            //Creating a Model usning the Event Data holer
            var eventDetails = new EventDetailsViewModel()
            {
                Id = eventData.Id,
                Firstname = userDataFirstName,
                LastName = userDataLastName,
                PhoneNumber = lePhone,
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
            var loggedInProfile = db.Profiles.SingleOrDefault(n => n.Id == loggedInUser.Profile.Id);

            var result = new GetEventViewModel()
            {
                Event = eventDetails,
                AlreadyBookedOnThisEvent = (loggedInUser == null) ? false : db.EventUsers.Any(n => n.EventId == eventData.Id && n.ProfileId == loggedInUser.Profile.Id),
                IsOwnerOfThisEvent = (loggedInUser == null) ? false : db.EventUsers.Any(n => n.EventId == eventData.Id && n.ProfileId == loggedInUser.Profile.Id && n.IsOwner),
                LoggedInProfile = loggedInProfile,

                //DEBUG
                BookedUsers = db.EventUsers.Where(n => n.EventId == eventData.Id && n.IsOwner == false).Select(n => n.Profile).ToList(),
                OwnerUsers = db.EventUsers.Where(n => n.EventId == eventData.Id && n.IsOwner).Select(n => n.Profile).ToList()

            };

            return View(result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BookEvent(BookEventViewModel bevm)
        {
            var loggedInUser = GetTheLoggedInUserAndCheckForNull();

            var theBooking = new EventUser()
            {
                EventId = bevm.EventId,
                ProfileId = loggedInUser.Profile.Id,
                Status = "Confirmed",
                IsOwner = false,
            };

            var alreadyBooked =
                db.EventUsers.Any(n => n.EventId == bevm.EventId && n.ProfileId == loggedInUser.Profile.Id);

            if (alreadyBooked)
                throw new Exception("Du är redan bokad på eventet.");

            db.EventUsers.Add(theBooking);
            db.SaveChanges();

            // TODO: Make this add the booking without changing page.
            return RedirectToAction("GetEvent", new { id = bevm.EventId });
        }


        // GET: Events/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
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

                return RedirectToAction("Index");
            }

            return View(evm);
        }

        private ApplicationUser GetTheLoggedInUserAndCheckForNull()
        {
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);

            if (loggedInUser == null)
                throw new Exception("Du måste vara inloggad för denna funktion!");
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

        public ActionResult GetSpotsLeft(int id)
        {
            //var result1 = db.EventUsers.Local.Count(s => s.EventId == id);
            var result1 = db.EventUsers.Count(s => s.EventId == id);
            var maxSignups = db.Events.Find(id).MaxSignups + 1;
            if (maxSignups == null) return PartialView((int?)null);
            var result = maxSignups.Value - result1;
            return PartialView("GetSpotsLeft", result);
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