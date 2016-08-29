using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DatabaseObjects;
using Webbsida.Models;

namespace Webbsida.Controllers.api
{
    public class GeoDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/GeoData
        public List<EventApiViewModel> GetEvents(string latitude, string longitude)
        {
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
                .Select(n => new {n.Id, n.Latitude, n.Longitude});

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

        //// GET: api/GeoData/5
        //[ResponseType(typeof(Event))]
        //public async Task<IHttpActionResult> GetEvent(int id)
        //{
        //    Event @event = await db.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(@event);
        //}

        //// PUT: api/GeoData/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutEvent(int id, Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != @event.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(@event).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EventExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/GeoData
        //[ResponseType(typeof(Event))]
        //public async Task<IHttpActionResult> PostEvent(Event @event)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Events.Add(@event);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = @event.Id }, @event);
        //}

        //// DELETE: api/GeoData/5
        //[ResponseType(typeof(Event))]
        //public async Task<IHttpActionResult> DeleteEvent(int id)
        //{
        //    Event @event = await db.Events.FindAsync(id);
        //    if (@event == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Events.Remove(@event);
        //    await db.SaveChangesAsync();

        //    return Ok(@event);
        //}

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

    //public class EventApiViewModel
    //{
    //    public int Id { get; set; }

    //    public string Name { get; set; }

    //    public double Latitude { get; set; }
    //    public double Longitude { get; set; }
    //}
}