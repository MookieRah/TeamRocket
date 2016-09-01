using System.Linq;
using System.Web.Mvc;
using DatabaseObjects;
using Microsoft.AspNet.Identity;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class MyEventsController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //my events
            var user = _db.Users.Find(User.Identity.GetUserId());
            var profileId = user.Profile.Id;

            var query = 
            (
                from eventUser in _db.EventUsers
                join @event in _db.Events
                    on eventUser.EventId equals @event.Id
                where eventUser.ProfileId == profileId
                select new IndexEventViewModel()
                {
                    Name = @event.Name,
                    Description = @event.Description,
                    ImagePath = @event.ImagePath,
                    StartDate = @event.StartDate,
                    EndDate = @event.EndDate,
                    Price = @event.Price,
                    Id = @event.Id,
                    Latitude = @event.Latitude,
                    Longitude = @event.Longitude,
                    MinSignups = @event.MinSignups,
                    MaxSignups = @event.MaxSignups,
                    EventUsers = @event.EventUsers,
                    OwnerId = @event.EventUsers.Where(x => x.IsOwner == true)
                    .Select(x => x.Profile).FirstOrDefault().Id
            }).ToList();


            var ownedEvents = query.Where(e => e.OwnerId == profileId)
                .Select(rawEvent => new IndexEventViewModel
            {
                Id = rawEvent.Id,
                EventUsers = rawEvent.EventUsers,
                Name = rawEvent.Name,
                Description = rawEvent.Description,
                StartDate = rawEvent.StartDate,
                EndDate = rawEvent.EndDate,
                Latitude = rawEvent.Latitude,
                Longitude = rawEvent.Longitude,
                Price = rawEvent.Price,
                ImagePath = rawEvent.ImagePath,
                MinSignups = rawEvent.MinSignups,
                MaxSignups = rawEvent.MaxSignups
            }).ToList();

            var bookedEvents = query.Where(e => e.OwnerId != profileId)
                .Select(rawEvent => new IndexEventViewModel
            {
                Id = rawEvent.Id,
                EventUsers = rawEvent.EventUsers,
                Name = rawEvent.Name,
                Description = rawEvent.Description,
                StartDate = rawEvent.StartDate,
                EndDate = rawEvent.EndDate,
                Latitude = rawEvent.Latitude,
                Longitude = rawEvent.Longitude,
                Price = rawEvent.Price,
                ImagePath = rawEvent.ImagePath,
                MinSignups = rawEvent.MinSignups,
                MaxSignups = rawEvent.MaxSignups
            }).ToList();

            var results = new MyEventsViewModel
            {
                UserName = user.UserName,
                EventsOwned = ownedEvents,
                EventsBooked = bookedEvents
            };

            return View(results);
        }
    }
}


//        //public ActionResult Index()
//        //{
//        //    return View(db.Events.ToList());
//        //}

//        // GET: test/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var @event = _db.Events.Find(id);
//            if (@event == null)
//            {
//                return HttpNotFound();
//            }
//            return View(@event);
//        }

//        // GET: test/Create
//        public ActionResult Create()
//        {
//            return View();
//        }

//        // POST: test/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create(
//            [Bind(
//                Include =
//                    "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] Event @event)
//        {
//            if (ModelState.IsValid)
//            {
//                _db.Events.Add(@event);
//                _db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            return View(@event);
//        }

//        // GET: test/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var @event = _db.Events.Find(id);
//            if (@event == null)
//            {
//                return HttpNotFound();
//            }
//            return View(@event);
//        }

//        // POST: test/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit(
//            [Bind(
//                Include =
//                    "Id,Name,Description,StartDate,EndDate,Latitude,Longitude,MaxSignups,MinSignups,Price,ImagePath")] Event @event)
//        {
//            if (ModelState.IsValid)
//            {
//                _db.Entry(@event).State = EntityState.Modified;
//                _db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            return View(@event);
//        }

//        // GET: test/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            var @event = _db.Events.Find(id);
//            if (@event == null)
//            {
//                return HttpNotFound();
//            }
//            return View(@event);
//        }

//        // POST: test/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            var @event = _db.Events.Find(id);
//            _db.Events.Remove(@event);
//            _db.SaveChanges();
//            return RedirectToAction("Index");
//        }
//    }
//}
