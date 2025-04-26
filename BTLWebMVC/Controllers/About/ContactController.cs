using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTLWebMVC.Controllers.About
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Latitude = 10.460411;
            ViewBag.Longitude = 105.635979;
            ViewBag.LocationName = "Cửa Hàng Thiết Bị Điện Tử HPH";
            return View();
        }
    }
}