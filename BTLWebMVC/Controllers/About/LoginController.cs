using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;

namespace BTLWebMVC.Controllers
{
    public class LoginController : Controller
    {
        private Context db = new Context();
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
        [HttpPost]
        public ActionResult Register(string username, string password, string email, string phone, string contactname)
        {
            return View();
        }
        public ActionResult Forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Loginnew(string username, string password)
        {
            var account = db.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (account == null)
            {
                return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng!" });
            }
            Session["AccountId"] = account.AccountID;
            Session["Role"] = account.Role;

            if (account.Role == "Admin" || account.Role == "Employee")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "HomeManager", new { id = account.AccountID }) });
            }
            else
            {
                return Json(new { success = true, reload = true });
            }
        }
    }
}