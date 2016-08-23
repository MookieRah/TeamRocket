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

        public EventController()
        {
        }

        // GET: Event
        public ActionResult Index()
        {
            return View(_db);
        }

        public ActionResult GetEvent(int id)
        {
            var eventData = _db.Events.Find(id);
            //var UsersData = _db.Profiles.Find(id);
            var result= new EventViewModel
            {   
                //Firstname = UsersData.FirstName.Where(),
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
            //Event ViewEvent = new Event();
            //List<ViewEvent> list = null;
            //try
            //{

            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return View(result);
        }
    }
}