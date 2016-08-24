using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events
        
        public ActionResult Index()
        {
            //the user object now has the form fields from the view. 
            /*
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file];
                string path = Server.MapPath("~/Images/" + hpf.FileName);
                hpf.SaveAs(path);
                ViewBag.Path = path;
            }
            */
            return View(db.Events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel ev)
        {

            //EventViewModel evm = HttpPostedFileBase
            string path = Server.MapPath("~/Content/Eventimages/" + ev.Image.FileName);
            ev.Image.SaveAs(path);
            var res = new Event()
            {
                Name = ev.Name,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                MinSignups = ev.MinSignups,
                MaxSignups = ev.MaxSignups,
                Price = ev.Price,
                Latitude = ev.Latitude,


                ImagePath = path
            };

            db.Events.Add(res);
            db.SaveChanges();
            return RedirectToAction("Index");
            //ViewBag.Path = path;
            //return View(new EventViewModel());
            /*
        foreach (string file in Request.Files)
        {
            HttpPostedFileBase hpf = Request.Files[file];
            string path = Server.MapPath("~/Images/" + hpf.FileName);
            hpf.SaveAs(path);
            ViewBag.Path = path;
        }
        */



            /*
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

    
            return View(@event);
      */
            return View();
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
