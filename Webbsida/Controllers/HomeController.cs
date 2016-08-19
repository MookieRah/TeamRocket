using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webbsida.Models;

namespace Webbsida.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var events = _db.Events.ToList();

            return View(events);
        }
    }
}