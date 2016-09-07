using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using DatabaseObjects;
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
            return View("RealIndex");
        }

        public JsonResult GetTagsAndNamesBySearch(string filter)
        {
            var tags = _db.Tags.Where(t => t.Name.Contains(filter)).Select(x => x.Name).Distinct().ToList();
            var names = _db.Events.Where(n => n.Name.Contains(filter)).Select(x => x.Name).Distinct().ToList();

            var tagNames = new List<string>();
            tagNames.AddRange(tags);
            tagNames.AddRange(names);

            return Json(tagNames, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEventsBySearch(string filter, double latitude = 0.0d, double longitude = 0.0d )
        {
            List<Event> rawEvents = new List<Event>();

            try
            {
                var rawEventsFromTags = filter == "" ? _db.Events.ToList() : _db.Events.Where(e => e.EventTags.Any(t => t.Tag.Name.StartsWith(filter))).ToList();
                var rawEventsFromName = filter == "" ? null : _db.Events.Where(e => e.Name.StartsWith(filter)).ToList();

                rawEvents.AddRange(rawEventsFromTags);

                if(rawEventsFromName != null)
                    rawEvents.AddRange(rawEventsFromName);

                var events = new List<IndexEventViewModel>();

                var ids = new List<int>();

                for(int i = 0; i < rawEvents.Count; i++)
                {
                    if (ids.Any())
                    {
                        foreach (var id in ids.Where(id => rawEvents.ElementAt(i).Id == id))
                        {
                            rawEvents.RemoveAt(i);
                        }
                    }

                    ids.Add(rawEvents.ElementAt(i).Id);

                    events.Add(new IndexEventViewModel
                    {
                        Id = rawEvents.ElementAt(i).Id,
                        EventUsers = rawEvents.ElementAt(i).EventUsers,
                        Name = rawEvents.ElementAt(i).Name,
                        Description = rawEvents.ElementAt(i).Description,
                        StartDate = rawEvents.ElementAt(i).StartDate,
                        EndDate = rawEvents.ElementAt(i).EndDate,
                        Latitude = rawEvents.ElementAt(i).Latitude,
                        Longitude = rawEvents.ElementAt(i).Longitude,
                        Price = rawEvents.ElementAt(i).Price,
                        ImagePath = rawEvents.ElementAt(i).ImagePath,
                        MinSignups = rawEvents.ElementAt(i).MinSignups,
                        MaxSignups = rawEvents.ElementAt(i).MaxSignups
                    });
                }

                var userLocation = new Location {Latitude = latitude, Longitude = longitude};

                foreach (var @event in events)
                {
                    @event.Distance = GetDistance(userLocation,
                        new Location() {Latitude = @event.Latitude, Longitude = @event.Longitude});
                }

                Debug.WriteLine(userLocation.Latitude + " " + userLocation.Longitude);

                var orderedEvents = events.OrderBy(e => e.GetOrder).ToList();

                if (orderedEvents.Count % 4 != 0)
                {
                    int toAdd = orderedEvents.Count % 4;

                    for (int i = 0; i < toAdd; i++)
                    {
                        orderedEvents.Add(new IndexEventViewModel());
                    }
                }

                return PartialView("_DisplayEventSummary", orderedEvents);
            }
            catch (Exception ex)
            {
                TempData["notice"] = "There has been a problem.";

                return PartialView("_Error", ex);
            }
        }

        public ActionResult GetProcessing()
        {
            return PartialView("_Processing");
        }

        //[HttpPost]
        //public ActionResult Index(string latitude, string longitude)
        //{
        //    double latitudeParsed;
        //    double longitudeParsed;
        //    if (!double.TryParse(latitude, out latitudeParsed))
        //        return View("Error");
        //    if (!double.TryParse(longitude, out longitudeParsed))
        //        return View("Error");


        //    var fromDb = _db.Events
        //        .Where(n => n.Latitude < latitudeParsed + 1 && n.Latitude > latitudeParsed - 1)
        //        .Where(n => n.Longitude < longitudeParsed + 1 && n.Longitude > longitudeParsed - 1)
        //        .Where(n => n.StartDate > DateTime.Now)
        //        .Select(n => new { n.Id, n.Latitude, n.Longitude });

        //    var resultDict = new Dictionary<int, double>();
        //    foreach (var happening in fromDb)
        //    {
        //        resultDict.Add(
        //            happening.Id,
        //            GetDistance(
        //                new Location() { Latitude = latitudeParsed, Longitude = longitudeParsed },
        //                new Location() { Latitude = happening.Latitude, Longitude = happening.Longitude }
        //                ));
        //    }

        //    var returnData = new List<DistanceViewModel>();
        //    foreach (var keyValuePair in resultDict.OrderBy(n => n.Value))
        //    {
        //        returnData.Add(new DistanceViewModel()
        //        {
        //            Id = keyValuePair.Key,
        //            Distance = keyValuePair.Value
        //        });
        //    }

        //    //TODO No need to make a Dictionary here, just include the Distance in the
        //    // IndexEventViewModel.cs, then sort it by Distance with linq.

        //    return View("RealIndex", returnData);
        //}

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