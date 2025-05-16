using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
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

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,Username,Password,Email,ProfileImage,CreatedDate,Role,IsLock")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
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