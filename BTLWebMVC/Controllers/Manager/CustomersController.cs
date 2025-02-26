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
            var customers = db.Customers.Include(c => c.Account);
            return View(customers.ToList());
        }
        public ActionResult Create()
        {
            
            return View();
        }

   
        
   

    }
}
