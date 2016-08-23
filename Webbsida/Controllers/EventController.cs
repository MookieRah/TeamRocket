using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.Design;
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
            //Create a Event Data holder
            var eventData = _db.Events.Find(id);
            //Creating a Model usning the Event Data holer
            var result= new EventViewModel
            {   
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
    }
}