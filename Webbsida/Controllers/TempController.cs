using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DatabaseObjects;
using Microsoft.AspNet.Identity;
using Webbsida.Models;

namespace Webbsida.Controllers
{
    public class TempController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Temp
        public ActionResult Index()
        {
            // -> Example of at least one way to interact with asp.net IDENTITY userdata:
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);
            


            string phone = string.Empty;
            if (loggedInUser != null)
            {
                phone = loggedInUser.PhoneNumber;
                loggedInUser.PhoneNumber = "123456";
                db.SaveChanges();

                Debug.WriteLine($"Old phone nr: {phone}");
                Debug.WriteLine($"New phone nr: {loggedInUser.PhoneNumber}");
            }
            return View(db.Profiles.ToList());
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
