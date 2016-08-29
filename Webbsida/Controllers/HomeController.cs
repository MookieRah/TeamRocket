using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string latitude, string longitude)
        {
            double latitudeParsed;
            double longitudeParsed;
            if (!double.TryParse(latitude, out latitudeParsed))
                return View("Error");
            if (!double.TryParse(longitude, out longitudeParsed))
                return View("Error");


            var fromDb = _db.Events
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
                        new Location() { Latitude = latitudeParsed, Longitude = longitudeParsed },
                        new Location() { Latitude = happening.Latitude, Longitude = happening.Longitude }
                        ));
            }

            var returnData = new List<DistanceViewModel>();
            foreach (var keyValuePair in resultDict.OrderBy(n => n.Value))
            {
                returnData.Add(new DistanceViewModel()
                {
                    Id = keyValuePair.Key,
                    Distance = keyValuePair.Value
                });
            }

            //TODO No need to make a Dictionary here, just include the Distance in the
            // IndexEventViewModel.cs, then sort it by Distance with linq.

            return View("RealIndex", returnData);
        }

        private double ToRad(double input)
        {
            return input * Math.PI / 180;
        }

        private double GetDistance(Location p1, Location p2)
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
    }
    public struct Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DistanceViewModel
    {
        public int Id { get; set; }
        public double Distance { get; set; }
    }
}