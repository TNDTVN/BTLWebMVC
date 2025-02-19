using BTLWebMVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTLWebMVC.Controllers
{
    public class HomeManagerController : Controller
    {
        private Context db = new Context();
        public ActionResult Index(int? id)
        {

            return View();
        }
    }
}