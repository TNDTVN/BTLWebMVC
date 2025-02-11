using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;

namespace BTLWebMVC.Controllers
{
    public class LoginController : Controller
    {
        private Context db = new Context();
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Check(string username, string password)
        {
            var account = db.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (account == null)
            {
                TempData["ErrorMessage"] = "Tên đăng nhập hoặc mật khẩu không đúng!";
                return View("Login");
            }

            Session["AccountId"] = account.AccountID;
            Session["Role"] = account.Role;
            TempData["SuccessMessage"] = "Bạn đã đăng nhập thành công!";
            if (account.Role == "Admin" || account.Role == "Employee")
            {
                return RedirectToAction("Index", "HomeManager", new { id = account.AccountID });
            }
            else if (account.Role == "Customer")
            {
                return RedirectToAction("Index", "Home", new { id = account.AccountID });
            }
            else
            {
                TempData["ErrorMessage"] = "Quyền không hợp lệ!";
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();

            TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công!";

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Forgotpassword()
        {
            return View();
        }
    }
}