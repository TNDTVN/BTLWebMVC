using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BTLWebMVC.App_Start;
using System.Web.Helpers;
using System.Threading.Tasks;
using BTLWebMVC.Models;

namespace BTLWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Images).Take(50).ToList();
            return View(products); 
        }

        public ActionResult About()
        {

            return View();
        }
        
        public ActionResult Contact()
        {

            return View();
        }
    }
}