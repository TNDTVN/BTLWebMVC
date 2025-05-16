using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using System.IO;
using BTLWebMVC.Models;

namespace BTLWebMVC.Controllers.Manager
{
    public class AccountsController : Controller
    {
        private Context db = new Context();


        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Employees).Include(a => a.Customers).ToList();
            return View(accounts);
        }


        // action  lock trang thiai tai khoan
        [HttpPost]
        public JsonResult ToggleLock(int id, int isLock)
        {
            try
            {
                var account = db.Accounts.Find(id);
                if (account == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy tài khoản!" });
                }

                account.IsLock = (isLock == 1);
                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật trạng thái: " + ex.Message });
            }
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách vai trò duy nhất từ CSDL
            var roles = db.Accounts.Select(a => a.Role).Distinct().ToList();
            ViewBag.Role = new SelectList(roles.Select(r => new SelectListItem
            {
                Text = r,
                Value = r
            }), "Value", "Text", account.Role);

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,Username,Password,Email,ProfileImage,CreatedDate,Role,IsLock")] Account account, HttpPostedFileBase FileAnh)
        {
            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.Find(account.AccountID);
                if (existingAccount == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin tài khoản
                existingAccount.Username = account.Username;
                existingAccount.Password = account.Password;
                existingAccount.Email = account.Email;
                existingAccount.Role = account.Role;
                existingAccount.IsLock = account.IsLock;

           
                if (FileAnh != null && FileAnh.ContentLength > 0)
                {
                  
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(FileAnh.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);

                    FileAnh.SaveAs(path);

                    existingAccount.ProfileImage = "/Content/Images/" + fileName;

                  
                    if (!string.IsNullOrEmpty(existingAccount.ProfileImage) && System.IO.File.Exists(Server.MapPath(existingAccount.ProfileImage)))
                    {
                        System.IO.File.Delete(Server.MapPath(existingAccount.ProfileImage));
                    }
                }

                db.Entry(existingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Lấy lại danh sách vai trò khi form không hợp lệ
            var roles = db.Accounts.Select(a => a.Role).Distinct().ToList();
            ViewBag.Role = new SelectList(roles.Select(r => new SelectListItem
            {
                Text = r,
                Value = r
            }), "Value", "Text", account.Role);

            return View(account);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}