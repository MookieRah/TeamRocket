using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
using Webbsida.Models;

namespace Webbsida.Controllers.api
{
    public class GeoDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GeoData
        public List<EventApiViewModel> GetEvents(string latitude, string longitude)
        {
            // TODO: Major bug with the geoLocation javascripts, all functions run on every page!!!!
            // TODO: Test the logic in this webapi-controller!
            double latitudeParsed;
            double longitudeParsed;
            if (!double.TryParse(latitude, out latitudeParsed))
                return new List<EventApiViewModel>(); //TODO: error handling
            if (!double.TryParse(longitude, out longitudeParsed))
                return new List<EventApiViewModel>(); //TODO: error handling


            var fromDb = db.Events
                .Where(n => n.Latitude < latitudeParsed + 1 && n.Latitude > latitudeParsed - 1)
                .Where(n => n.Longitude < longitudeParsed + 1 && n.Longitude > longitudeParsed - 1)
                .Where(n => n.StartDate > DateTime.Now)
                .Select(n => new { n.Id, n.Latitude, n.Longitude });

            var resultDict = new Dictionary<int, double>();
            foreach (var happening in fromDb)
            {
                resultDict.Add(
                    happening.Id,
                    GetDistance(
                        new Point() { Latitude = latitudeParsed, Longitude = longitudeParsed },
                        new Point() { Latitude = happening.Latitude, Longitude = happening.Longitude }
                        ));
            }

            var returnData = new List<EventApiViewModel>();
            foreach (var keyValuePair in resultDict.OrderBy(n => n.Value))
            {
                returnData.Add(new EventApiViewModel()
                {
                    Id = keyValuePair.Key,
                    Distance = keyValuePair.Value
                });
            }

            return returnData;
        }

        [HttpGet]
        public HttpResponseMessage RequestAddressFromCoordinates(double latitude, double longitude)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false", // &language=sv || &language=se
                Uri.EscapeDataString(latitude.ToString(CultureInfo.InvariantCulture)), Uri.EscapeDataString(longitude.ToString(CultureInfo.InvariantCulture)));


            string addressResult = string.Empty;

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;

                try
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
                catch (Exception)
                {
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("", Encoding.UTF8, "text/html")
                    };
                }
            }

            return new HttpResponseMessage()
            {
                Content = new StringContent(addressResult, Encoding.UTF8, "text/html")
            };

        }

        private double ToRad(double input)
        {
            return input * Math.PI / 180;
        }

        private double GetDistance(Point p1, Point p2)
        {
            var R = 6378137; // Earth’s mean radius in meter
            var dLat = ToRad(p2.Latitude - p1.Latitude);
            var dLong = ToRad(p2.Longitude - p1.Longitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(ToRad(p1.Latitude)) * Math.Cos(ToRad(p2.Latitude)) *
              Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d; // returns the distance in meter
        }

        public struct Point
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }
    }

    public class EventApiViewModel
    {
        public int Id { get; set; }
        public double Distance { get; set; }
    }

}