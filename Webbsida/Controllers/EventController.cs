using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DatabaseObjects;
using Microsoft.ApplicationInsights.WindowsServer;
using Microsoft.AspNet.Identity;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class EventController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Events

        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Events/Details
        public ActionResult Details(int id)
        {
            var result = db.Events.SingleOrDefault(n => n.Id == id);
            return View(result);
        }

        // GET: Event
        public ActionResult GetEvent(int id)
        {
            //Create a Data holders
            var eventData = db.Events.Find(id);

            var lePhone = db.Users
                .Where(s => s.Profile.Id == id)
                .Select(p => p.PhoneNumber).SingleOrDefault();

            var eventUserData = db.EventUsers
                .Where(d => d.EventId == id)
                .Select(g => g.EventId).FirstOrDefault();

            var userDataFirstName =
                db.Profiles.Where(d => d.Id == eventUserData).Select(f => f.FirstName).SingleOrDefault();
            var userDataLastName =
                db.Profiles.Where(d => d.Id == eventUserData).Select(f => f.LastName).SingleOrDefault();



            //Creating a Model usning the Event Data holer
            var eventDetails = new EventDetailsViewModel()
            {
                Id = eventData.Id,
                Firstname = userDataFirstName,
                LastName = userDataLastName,
                PhoneNumber = lePhone,
                ImagePath = eventData.ImagePath,
                EventName = eventData.Name,
                Description = eventData.Description,
                StartDate = eventData.StartDate,
                EndDate = eventData.EndDate,
                Latitude = eventData.Latitude,
                Longitude = eventData.Longitude,
                MaxSignups = eventData.MaxSignups,
                MinSignups = eventData.MinSignups,
                Price = eventData.Price
            };
            var loggedInUserId = User.Identity.GetUserId();
            var loggedInUser = db.Users.SingleOrDefault(n => n.Id == loggedInUserId);

            var result = new GetEventViewModel()
            {
                Event = eventDetails,
                LoggedInUser = loggedInUser
            };

            return View(result);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEventViewModel ev)
        {
            // TODO: Model-Validation (and optional file uploaded)!

            if (ev.Image.ContentLength > 3000000)
                ModelState.AddModelError("", "Max 3 mb!");

            if (ModelState.IsValid)
            {
                // TODO: Use a default image if none is supplied by the user.
                // TODO: Make sure this path will be correct in the db!!
                var path = Path.Combine(Server.MapPath("/Content/EventImages/"), ev.Image.FileName);

                var fileExtension = Path.GetExtension(ev.Image.FileName).ToLower();
                if (fileExtension == ".png" || fileExtension == ".jpg" || fileExtension == ".gif" || fileExtension == ".jpeg" || fileExtension == ".jpe" || fileExtension == ".jfif")
                {
                    ev.Image.SaveAs(path);


                // TODO: Connect with path instead.
                string pathToSaveInDb = @"\Content\EventImages\" + ev.Image.FileName;
                // Shall update filetype

                var result = new Event()
                {
                    Name = ev.Name,
                    Description = ev.Description,
                    StartDate = ev.StartDate,
                    EndDate = ev.EndDate,
                    MinSignups = ev.MinSignups,
                    MaxSignups = ev.MaxSignups,
                    Price = ev.Price,
                    Latitude = ev.Latitude,
                    Longitude = ev.Longitude,

                    ImagePath = pathToSaveInDb
                };
                db.Events.Add(result);
                db.SaveChanges();

                return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Bilden måste vara någon av följande typer; .png, .jpg, .gif, .jpeg, .jpe, .jfif");
                    //return Content("<script language='javascript' type='text/javascript'>alert('We dont accept your filetype. The filetype we ccept is .PNG, .GIF, .JPG.');</script>");
                }
            }

            return View(ev);
        }


        public ActionResult GetSpotsLeft(int id)
        {
            //var result1 = db.EventUsers.Local.Count(s => s.EventId == id);
            var result1 = db.EventUsers.Count(s => s.EventId == id);
            var maxSignups = db.Events.Find(id).MaxSignups+1;
            if (maxSignups == null) return PartialView((int?)null);
            var result = maxSignups.Value - result1;
            return PartialView("GetSpotsLeft", result);
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