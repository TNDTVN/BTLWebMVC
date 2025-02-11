using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BTLWebMVC.App_Start;
using System.Web.Helpers;
using System.Threading.Tasks;

namespace BTLWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(int? id)
        {
            var product = db.Products.Take(50);
            return View(product);
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