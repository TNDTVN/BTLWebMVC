using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BTLWebMVC.App_Start;
using System.Web.Helpers;

namespace BTLWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult KiemTra(string username, string password)
        {
            var taiKhoan = _context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (taiKhoan == null) 
            {
                if (taiKhoan.Role == "Admin" || taiKhoan.Role == "Employee")
                {

                }
                else if(taiKhoan.Role == "Customer")
                { 
                
                }
            }

            return View("Index");
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