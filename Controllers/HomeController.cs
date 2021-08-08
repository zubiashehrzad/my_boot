using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using my_boot.Models;

namespace my_boot.Controllers
{
    public class HomeController : Controller
    {

        private MyCartEntities db = new MyCartEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        internal class Item
        {
            public object Product { get; internal set; }
        }
    }
}