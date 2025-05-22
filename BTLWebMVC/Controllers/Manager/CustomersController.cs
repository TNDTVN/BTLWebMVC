using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;

namespace BTLWebMVC.Controllers
{
    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index()
        {
            ViewBag.CurrentPage = "Customers";
            var customers = db.Customers.Include(c => c.Account).ToList();
            return View(customers);
        }

        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Customers";
            return View();
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
