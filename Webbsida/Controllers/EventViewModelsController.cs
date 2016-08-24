using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class EventViewModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EventViewModels
        public ActionResult Index()
        {
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(EventViewModel events)
        {
            //the user object now has the form fields from the view. 

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file];
                string path = Server.MapPath("~/Images/" + hpf.FileName);
                hpf.SaveAs(path);
                ViewBag.Path = path;
            }

            return View();
        }
        // GET: EventViewModels/Details/5
        /*
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventViewModel eventViewModel = db.EventViewModels.Find(id);
            if (eventViewModel == null)
            {
                return HttpNotFound();
            }
            
            return View(eventViewModel);
        }
        */
        // GET: EventViewModels/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/Images/"+file.FileName);
            file.SaveAs(path);
            ViewBag.Path = path;
            return View();
        }
        // POST: EventViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                db.EventViewModels.Add(eventViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eventViewModel);
        }
        */
        // GET: EventViewModels/Edit/5
        /*
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventViewModel eventViewModel = db.EventViewModels.Find(id);
            if (eventViewModel == null)
            {
                return HttpNotFound();
            }
            return View(eventViewModel);
        }
        */
        // POST: EventViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eventViewModel);
        }

        // GET: EventViewModels/Delete/5

        /*
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        EventViewModel eventViewModel = db.EventViewModels.Find(id);
        if (eventViewModel == null)
        {
            return HttpNotFound();
        }
        return View(eventViewModel);
    }
    */
        // POST: EventViewModels/Delete/5
        /*
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventViewModel eventViewModel = db.EventViewModels.Find(id);
            db.EventViewModels.Remove(eventViewModel);
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
        }*/
    }
}
