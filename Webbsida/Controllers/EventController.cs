using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DatabaseObjects;
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

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEventViewModel ev)
        {
            // TODO: Model-Validation (and optional file uploaded)!

            // TODO: Make sure this path will be correct in the db!!
            var path = Path.Combine(Server.MapPath("/Content/EventImages/"), ev.Image.FileName);
            ev.Image.SaveAs(path);

            // TODO: Connect with path instead.
            string pathToSaveInDb = @"\Content\EventImages\" + ev.Image.FileName;
            // Shall update filetype

            var res = new Event()
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

            db.Events.Add(res);
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