using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbsida.Models;
using Webbsida.ViewModels;

namespace Webbsida.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var eventDbos = _db.Events.ToList();

            var events = eventDbos.Select(eventDbo => new IndexEventViewModel(eventDbo)).ToList();

            return View(events);
        }
    }
}