using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BTLWebMVC.Controllers.User
{
    public class UserAcountController : Controller
    {
        Context db = new Context();

        // GET: UserAcount
        public ActionResult Index()
        {
            if (Session["AccountId"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem thông tin tài khoản.";
                return RedirectToAction("Index", "Login");
            }

            int accountId;
            if (!int.TryParse(Session["AccountId"]?.ToString(), out accountId))
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Index", "Login");
            }

            var customer = db.Customers
                .Include("Account")
                .FirstOrDefault(c => c.AccountID == accountId);

            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin tài khoản.";
                return RedirectToAction("Index", "Login");
            }

            return View(customer);
        }

        // POST: Update user information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer model, HttpPostedFileBase profileImage)
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập để cập nhật thông tin." });
            }

            int accountId;
            if (!int.TryParse(Session["AccountId"]?.ToString(), out accountId))
            {
                return Json(new { success = false, message = "Phiên đăng nhập không hợp lệ." });
            }

            var customer = db.Customers
                .Include("Account")
                .FirstOrDefault(c => c.AccountID == accountId);

            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin tài khoản." });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update customer information
                    customer.CustomerName = model.CustomerName;
                    customer.ContactName = model.ContactName;
                    customer.Address = model.Address;
                    customer.City = model.City;
                    customer.PostalCode = model.PostalCode;
                    customer.Country = model.Country;
                    customer.Phone = model.Phone;
                    customer.Email = model.Email;

                    // Update Account email if it exists
                    if (customer.Account != null)
                    {
                        customer.Account.Email = model.Email;
                        db.Entry(customer.Account).State = EntityState.Modified;
                    }

                    // Handle profile image upload
                    if (profileImage != null && profileImage.ContentLength > 0)
                    {
                        // Delete old image if it exists and is not profile.jpg
                        if (!string.IsNullOrEmpty(customer.Account?.ProfileImage) && customer.Account.ProfileImage != "profile.jpg")
                        {
                            string oldImagePath = Path.Combine(Server.MapPath("~/Content/accountImages"), customer.Account.ProfileImage);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        string fileName = Path.GetFileNameWithoutExtension(profileImage.FileName) + "_" + DateTime.Now.Ticks + Path.GetExtension(profileImage.FileName);
                        string path = Path.Combine(Server.MapPath("~/Content/accountImages"), fileName);
                        profileImage.SaveAs(path);
                        customer.Account.ProfileImage = fileName;
                        db.Entry(customer.Account).State = EntityState.Modified;
                    }

                    // Mark customer entity as modified
                    db.Entry(customer).State = EntityState.Modified;

                    // Save changes to database
                    db.SaveChanges();

                    return Json(new { success = true, message = "Cập nhật thông tin thành công!" });
                }
                catch (DbUpdateException ex)
                {
                    return Json(new { success = false, message = "Lỗi khi cập nhật thông tin: " + (ex.InnerException?.Message ?? ex.Message) });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
                }
            }
            else
            {
                // Collect ModelState errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Dữ liệu nhập không hợp lệ: " + string.Join("; ", errors) });
            }
        }
    }
}