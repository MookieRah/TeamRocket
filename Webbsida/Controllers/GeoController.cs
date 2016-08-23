using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.Controllers
{
    public class GeoController : Controller //TODO: IDisposable!
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

        [HttpGet]
        public JsonResult RequestCoordinatesFromAddress(string address)
        {
            //var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            //var request = WebRequest.Create(requestUri);
            //var response = request.GetResponse();
            //var xdoc = XDocument.Load(response.GetResponseStream());

            //var result = xdoc.Element("GeocodeResponse").Element("result");

            //if (result != null)
            //{
            //    var locationElement = result.Element("geometry").Element("location");
            //    var lat = locationElement.Element("lat").Value;
            //    var lng = locationElement.Element("lng").Value;

            //    Console.WriteLine($"lat: {lat}, long: {lng}");
            //}
            //else
            //{
            //    Console.WriteLine("Could not find that city.");
            //}

            var result = new Coordinate()
            {
                Latitude = 666,
                Longitude = 777
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}