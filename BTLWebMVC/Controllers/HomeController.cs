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
using PagedList;

namespace BTLWebMVC.Controllers
{
    public class HomeController : Controller
    {

        private Context db = new Context();

        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;

            var products = db.Products.Include(p => p.Images).Include(p => p.Category)
                                      .OrderByDescending(x => x.ProductID);
            var categories = db.Categories.ToList(); 
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.Categories = categories;
            return View(products.ToPagedList(pageNumber, pageSize));
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