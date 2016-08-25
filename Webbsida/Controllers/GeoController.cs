using System;
using System.Collections.Generic;
using System.Globalization;
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

        [HttpGet]
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
        public JsonResult GetSingleEventToJson(int id)
        {
            var eventInDb = db.Events.SingleOrDefault(n => n.Id == id);

            if (eventInDb == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            var result = new Event()
            {
                Longitude = eventInDb.Longitude,
                Latitude = eventInDb.Latitude,
                Name = eventInDb.Name
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RequestCoordinatesFromAddress(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var coordinatesResult = new Coordinate();

            var result = xdoc.Element("GeocodeResponse").Element("result");

            if (result != null)
            {
                var locationElement = result.Element("geometry").Element("location");

                coordinatesResult.Latitude = locationElement.Element("lat").Value;
                coordinatesResult.Longitude = locationElement.Element("lng").Value;
            }
            else
            {
                coordinatesResult.Latitude = "error";
                coordinatesResult.Longitude = "error";
            }


            return Json(coordinatesResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RequestAddressFromCoordinates(double lat, double lon)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false",
                Uri.EscapeDataString(lat.ToString(CultureInfo.InvariantCulture)), Uri.EscapeDataString(lon.ToString(CultureInfo.InvariantCulture)));


            string addressResult = string.Empty;

            using (WebClient wc = new WebClient())
            {
                string result = wc.DownloadString(requestUri);
                var xmlElm = XElement.Parse(result);
                var status = (from elm in xmlElm.Descendants()
                              where
                                elm.Name == "status"
                              select elm).FirstOrDefault();
                if (status.Value.ToLower() == "ok")
                {
                    var res = (from elm in xmlElm.Descendants()
                               where
                                elm.Name == "formatted_address"
                               select elm).FirstOrDefault();

                    addressResult = res.Value;
                }
            }


            return Json(addressResult, JsonRequestBehavior.AllowGet);
        }

    }

    // TODO: MOVE THIS, only debug
    public struct Coordinate
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}