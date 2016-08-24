using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.Controllers
{
    public class EventController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View(_db);
        }

        // GET: Event
        public ActionResult GetEvent(int id)
        {
            //Create a Data holders
            var eventData = _db.Events.Find(id);
            
            
            //Creating a Model usning the Event Data holer
            var result= new EventViewModel
            {
                
                ImagePath = eventData.ImagePath,
                EventName = eventData.Name,
                Description = eventData.Description,
                StartDate = eventData.StartDate,
                EndDate = eventData.EndDate,
                Latitude = eventData.Latitude,
                Longitude = eventData.Longitude,
                MaxSignups = eventData.MaxSignups,
                MinSignups = eventData.MinSignups,
                Price = eventData.Price

            };
            return View(result);
        }

        public ActionResult GetSpotsLeft(int id)
        {
            var result1 = _db.EventUsers.Local.Count(s => s.EventId == id);
            var maxSignups = _db.Events.Find(id).MaxSignups;
            if (maxSignups == null) return PartialView((int?) null);
            var result = maxSignups.Value - result1;
            return PartialView("GetSpotsLeft", result);
        }
    }
}