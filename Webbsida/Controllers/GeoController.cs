using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.Controllers
{
    public class GeoController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Geo
        public ActionResult Index()
        {
            return View();
        }

        // GET: GeoPicker
        public ActionResult GeoPicker()
        {
            return View();
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetEventsToJson()
        {
            List<Event> result = new List<Event>();
            var eventsInDb = db.Events.ToList();
            foreach (var @event in eventsInDb)
            {
                result.Add(new Event()
                {
                    Longitude = @event.Longitude,
                    Latitude = @event.Latitude,
                    Name = @event.Name
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}