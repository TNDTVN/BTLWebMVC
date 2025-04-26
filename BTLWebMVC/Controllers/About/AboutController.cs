using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTLWebMVC.App_Start;
using System.Web.Mvc;
using System.Web.Helpers;

namespace BTLWebMVC.Controllers.About
{
    public class AboutController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" }, JsonRequestBehavior.AllowGet);
            }

            int accountId = (int)Session["AccountId"];
            var employee = db.Employees.FirstOrDefault(c => c.AccountID == accountId);
            if (employee == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin nhân viên!" }, JsonRequestBehavior.AllowGet);
            }

            string imageName = employee.Account.ProfileImage;
            string imagePath = Server.MapPath("~/Content/accountImages/" + imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return HttpNotFound("Ảnh không tồn tại!");
            }

            return File(imagePath, "image/jpeg"); 
        }

        public JsonResult Name()
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" }, JsonRequestBehavior.AllowGet);
            }

            int accountId = (int)Session["AccountId"];
            var employee = db.Employees.FirstOrDefault(c => c.AccountID == accountId);
            if (employee == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin nhân viên!" }, JsonRequestBehavior.AllowGet);
            }

            string fullName = $"{employee.FirstName} {employee.LastName}";

            return Json(new { success = true, message = fullName }, JsonRequestBehavior.AllowGet);
        }
    }
}