using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MainProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProjectStatus()
        {
            ViewBag.Message = "Project Status Page.";

            return View();
        }
        public ActionResult FinResources()
        {
            ViewBag.Message = "Financial Resources page.";

            return View();
        }
    }
}