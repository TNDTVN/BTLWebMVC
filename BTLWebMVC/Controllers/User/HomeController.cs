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

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Images)
                                      .Include(p => p.Category)
                                      .OrderByDescending(x => x.ProductID)
                                      .Take(20) 
                                      .ToList();

            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            return View(products); 
        }

        public ActionResult categories(int? page)
        {
            int pageSize = 16;
            int pageNumber = (page ?? 1); 

            var products = db.Products.OrderBy(p => p.ProductID)
                .Include(p => p.Images)
                .Include (p => p.Category)
                .ToPagedList(pageNumber, pageSize);

            return View(products);
        }
        public ActionResult Details(int id) 
        { 
            return View();
        }
    }
}