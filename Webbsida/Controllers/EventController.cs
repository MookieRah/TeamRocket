using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.Design;
using DatabaseObjects;

namespace Webbsida.Controllers
{
    public class EventController : Controller
    {
        public EventController()
        {
        }

        // GET: Event
        public ActionResult Index()
        {
            var entreas = new Event();
            return View(entreas);
        }

        //public ActionResult GetEvent(int id)
        //{
        //   Event ViewEvent = new Event();
        //    List<ViewEvent> list = null;
        //    try
        //    {
                
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}
    }
}