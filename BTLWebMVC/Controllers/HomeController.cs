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

        public HomeController()
        {
            _context = new Context(); 
        }
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
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                ViewBag.ShowSignInModal = true; 
                return View("Index");
            }

            Session["AccountId"] = taiKhoan.AccountID;
            Session["Role"] = taiKhoan.Role;

            if (taiKhoan.Role == "Admin" || taiKhoan.Role == "Employee")
            {
                return RedirectToAction("Index");
            }
            else if (taiKhoan.Role == "Customer")
            {
                return RedirectToAction("Index", "Customer");
            }

            return RedirectToAction("Index");
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