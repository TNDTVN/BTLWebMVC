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
        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword, string newReEnterPassword) 
        {
            var accountId = Session["AccountId"];
            if (accountId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }
            int id = Convert.ToInt32(accountId);
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }
            if (account.Password != oldPassword)
            {
                return Json(new { success = false, message = "Mật khẩu hiện tại không chính xác!" });
            }
            if (newPassword != newReEnterPassword)
            {
                return Json(new { success = false, message = "Mật khẩu mới và mật khẩu nhập lại không chính xác!" });
            }
            if (account.Password == newPassword)
            {
                return Json(new { success = false, message = "Vui lòng nhập mật khẩu mới và mật khẩu cũ khác nhau!" });
            }
            if (newPassword.Length < 6 || newReEnterPassword.Length < 6)
            {
                return Json(new { success = false, message = "Vui lòng nhập mật khẩu mới dài hơn 6 ký tự!" });
            }
            account.Password = newPassword;
            db.SaveChanges();
            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
        }
    }
}