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

namespace Webbsida.Controllers
{
    public class CompanyInformationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CompanyInformations
        public ActionResult Index()
        {
            return View(db.CompanyInformations.ToList());
        }

        // GET: CompanyInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInformation companyInformation = db.CompanyInformations.Find(id);
            if (companyInformation == null)
            {
                return HttpNotFound();
            }
            return View(companyInformation);
        }

        // GET: CompanyInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,Description,Longitude,Altitude,Phonenumber")] CompanyInformation companyInformation)
        {
            if (ModelState.IsValid)
            {
                db.CompanyInformations.Add(companyInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(companyInformation);
        }

        // GET: CompanyInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInformation companyInformation = db.CompanyInformations.Find(id);
            if (companyInformation == null)
            {
                return HttpNotFound();
            }
            return View(companyInformation);
        }

        // POST: CompanyInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,Description,Longitude,Altitude,Phonenumber")] CompanyInformation companyInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(companyInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(companyInformation);
        }

        // GET: CompanyInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyInformation companyInformation = db.CompanyInformations.Find(id);
            if (companyInformation == null)
            {
                return HttpNotFound();
            }
            return View(companyInformation);
        }

        // POST: CompanyInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CompanyInformation companyInformation = db.CompanyInformations.Find(id);
            db.CompanyInformations.Remove(companyInformation);
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
