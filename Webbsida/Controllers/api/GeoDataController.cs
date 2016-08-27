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
        public IEnumerable<EventApiViewModel> GetEvents()
        {
            // TODO: Don't include events to far away (a couple of degrees maybe)
            // TODO: Don't include inactive events

            // TODO: Try the Distance calc serverside instead!
            var fromDb = db.Events;
            var results = new List<EventApiViewModel>();
            foreach (var happening in fromDb)
            {
                results.Add(new EventApiViewModel()
                {
                    Id = happening.Id,
                    Name = happening.Name,
                    Latitude = happening.Latitude,
                    Longitude = happening.Longitude
                });
            }
            return results.AsEnumerable();
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
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}