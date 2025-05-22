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
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core; 
namespace BTLWebMVC.Controllers.Manager
{
    public class AccountsController : Controller
    {
        private Context db = new Context();


        // GET: Accounts
        public ActionResult Index()
        {
            ViewBag.CurrentPage = "Accounts";
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
                if (account.Role == "Admin")
                {
                    return Json(new { success = false, message = "Không thể thay đổi tài khoản quản lý!" });
                }
                if (account.IsLock == (isLock == 1)) // Kiểm tra nếu trạng thái không thay đổi
                {
                    return Json(new { success = true, message = "Trạng thái không thay đổi!" });
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
            ViewBag.CurrentPage = "Accounts";
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "AccountID,Username,Password,Email,ProfileImage,CreatedDate,Role,IsLock")] Account account, HttpPostedFileBase FileAnh)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var existingAccount = db.Accounts.Find(account.AccountID);
        //        if (existingAccount == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        // Cập nhật thông tin tài khoản
        //        existingAccount.Username = account.Username;
        //        existingAccount.Password = account.Password;
        //        existingAccount.Email = account.Email;
        //        existingAccount.Role = account.Role;
        //        existingAccount.IsLock = account.IsLock;

        //        // Xử lý upload ảnh
        //        if (FileAnh != null && FileAnh.ContentLength > 0)
        //        {
        //            System.Diagnostics.Debug.WriteLine("FileAnh được gửi: " + FileAnh.FileName);
        //            string oldImagePath = existingAccount.ProfileImage;
        //            string fileExtension = Path.GetExtension(FileAnh.FileName).ToLower();

        //            // Kiểm tra định dạng file
        //            if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(fileExtension))
        //            {
        //                ModelState.AddModelError("FileAnh", "Chỉ hỗ trợ file ảnh .jpg, .jpeg, .png, .gif");
        //                System.Diagnostics.Debug.WriteLine("Lỗi định dạng file: " + fileExtension);
        //            }
        //            else
        //            {
        //                // Tạo tên file mới
        //                string fileName = Guid.NewGuid().ToString() + fileExtension;
        //                string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
        //                string relativePath = "/Content/Images/" + fileName;

        //                // Kiểm tra độ dài đường dẫn
        //                if (relativePath.Length > 255)
        //                {
        //                    ModelState.AddModelError("FileAnh", "Đường dẫn ảnh quá dài, vui lòng chọn file có tên ngắn hơn.");
        //                    System.Diagnostics.Debug.WriteLine("Lỗi độ dài đường dẫn: " + relativePath.Length);
        //                }
        //                else
        //                {
        //                    // Lưu file
        //                    FileAnh.SaveAs(path);
        //                    existingAccount.ProfileImage = relativePath;
        //                    System.Diagnostics.Debug.WriteLine("Cập nhật ProfileImage: " + relativePath);

        //                    // Xóa ảnh cũ
        //                    if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(Server.MapPath(oldImagePath)))
        //                    {
        //                        System.IO.File.Delete(Server.MapPath(oldImagePath));
        //                        System.Diagnostics.Debug.WriteLine("Xóa ảnh cũ: " + oldImagePath);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            System.Diagnostics.Debug.WriteLine("Không có file ảnh được gửi hoặc file rỗng.");
        //        }

        //        // Cập nhật CSDL
        //        db.Entry(existingAccount).State = EntityState.Modified;
        //        try
        //        {
        //            db.SaveChanges();
        //            System.Diagnostics.Debug.WriteLine("Lưu CSDL thành công, ProfileImage: " + existingAccount.ProfileImage);
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Lỗi khi lưu CSDL: " + ex.ToString());
        //            System.Diagnostics.Debug.WriteLine("Inner Exception: " + ex.InnerException?.ToString());
        //            ModelState.AddModelError("", "Lỗi khi lưu vào CSDL: " + (ex.InnerException?.Message ?? ex.Message));
        //        }
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine("ModelState không hợp lệ: " + string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
        //    }

        //    // Lấy lại danh sách vai trò khi form không hợp lệ
        //    var roles = db.Accounts.Select(a => a.Role).Distinct().ToList();
        //    ViewBag.Role = new SelectList(roles.Select(r => new SelectListItem
        //    {
        //        Text = r,
        //        Value = r
        //    }), "Value", "Text", account.Role);

        //    return View(account);
        //}

        [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit([Bind(Include = "AccountID,Username,Password,Email,ProfileImage,CreatedDate,Role,IsLock")] Account account, HttpPostedFileBase FileAnh)
{
    System.Diagnostics.Debug.WriteLine($"Bắt đầu Edit, AccountID: {account.AccountID}");

    if (ModelState.IsValid)
    {
        System.Diagnostics.Debug.WriteLine("ModelState hợp lệ, bắt đầu xử lý cập nhật.");
        var existingAccount = db.Accounts.Find(account.AccountID);
        if (existingAccount == null)
        {
            System.Diagnostics.Debug.WriteLine("Không tìm thấy tài khoản với AccountID: " + account.AccountID);
            return HttpNotFound();
        }

        // Cập nhật thông tin tài khoản
        System.Diagnostics.Debug.WriteLine($"Cập nhật thông tin tài khoản: Username={account.Username}, Email={account.Email}, Role={account.Role}, IsLock={account.IsLock}");
        existingAccount.Username = account.Username;
        existingAccount.Password = account.Password;
        existingAccount.Email = account.Email;
        existingAccount.Role = account.Role;
        existingAccount.IsLock = account.IsLock;

        // Xử lý upload ảnh
        if (FileAnh != null && FileAnh.ContentLength > 0)
        {
            System.Diagnostics.Debug.WriteLine($"File ảnh được gửi: FileName={FileAnh.FileName}, ContentLength={FileAnh.ContentLength}");
            string oldImagePath = existingAccount.ProfileImage;
            string fileExtension = Path.GetExtension(FileAnh.FileName).ToLower();

            // Kiểm tra định dạng file
            if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(fileExtension))
            {
                System.Diagnostics.Debug.WriteLine("Lỗi định dạng file: " + fileExtension);
                ModelState.AddModelError("FileAnh", "Chỉ hỗ trợ file ảnh .jpg, .jpeg, .png, .gif");
            }
            else
            {
                // Tạo tên file mới
                string fileName = Guid.NewGuid().ToString() + fileExtension;
                string path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                string relativePath = "/Content/Images/" + fileName;

                // Kiểm tra độ dài đường dẫn
                if (relativePath.Length > 255)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi độ dài đường dẫn: {relativePath.Length}");
                    ModelState.AddModelError("FileAnh", "Đường dẫn ảnh quá dài, vui lòng chọn file có tên ngắn hơn.");
                }
                else
                {
                    // Lưu file
                    try
                    {
                        FileAnh.SaveAs(path);
                        System.Diagnostics.Debug.WriteLine($"Lưu file ảnh thành công: {path}");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi lưu file ảnh: {ex.Message}");
                        ModelState.AddModelError("FileAnh", "Lỗi khi lưu file ảnh: " + ex.Message);
                        return View(account);
                    }

                    // Cập nhật đường dẫn ảnh mới
                    existingAccount.ProfileImage = relativePath;
                    System.Diagnostics.Debug.WriteLine($"Cập nhật ProfileImage: {relativePath}");

                    // Xóa ảnh cũ
                    if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(Server.MapPath(oldImagePath)))
                    {
                        try
                        {
                            System.IO.File.Delete(Server.MapPath(oldImagePath));
                            System.Diagnostics.Debug.WriteLine($"Xóa ảnh cũ thành công: {oldImagePath}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Lỗi khi xóa ảnh cũ: {ex.Message}");
                        }
                    }
                }
            }
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Không có file ảnh được gửi hoặc file rỗng.");
            // Nếu cần bắt buộc chọn file ảnh, thêm lỗi vào ModelState
            // ModelState.AddModelError("FileAnh", "Vui lòng chọn một file ảnh.");
        }

        // Cập nhật CSDL
        db.Entry(existingAccount).State = EntityState.Modified;
        System.Diagnostics.Debug.WriteLine($"Trạng thái trước khi lưu: ProfileImage={existingAccount.ProfileImage}, Username={existingAccount.Username}");
        try
        {
            db.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Lưu CSDL thành công.");
            return RedirectToAction("Index");
        }
        catch (EntityCommandExecutionException cmdEx)
        {
            System.Diagnostics.Debug.WriteLine($"EntityCommandExecutionException: {cmdEx.Message}");
            System.Diagnostics.Debug.WriteLine($"Inner Exception: {cmdEx.InnerException?.ToString() ?? "Không có inner exception"}");
            ModelState.AddModelError("", $"Lỗi khi thực thi lệnh CSDL: {cmdEx.InnerException?.Message ?? cmdEx.Message}");
        }
        catch (DbUpdateException dbEx)
        {
            System.Diagnostics.Debug.WriteLine($"DbUpdateException: {dbEx.Message}");
            System.Diagnostics.Debug.WriteLine($"Inner Exception: {dbEx.InnerException?.ToString() ?? "Không có inner exception"}");
            ModelState.AddModelError("", $"Lỗi khi lưu vào CSDL: {dbEx.InnerException?.Message ?? dbEx.Message}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Lỗi chung: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.ToString() ?? "Không có inner exception"}");
            ModelState.AddModelError("", $"Lỗi khi lưu vào CSDL: {ex.Message}");
        }
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("ModelState không hợp lệ: " + string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
    }

    // Lấy lại danh sách vai trò khi form không hợp lệ
    System.Diagnostics.Debug.WriteLine("Tải lại danh sách vai trò cho ViewBag.");
    var roles = db.Accounts.Select(a => a.Role).Distinct().ToList();
    ViewBag.Role = new SelectList(roles.Select(r => new SelectListItem
    {
        Text = r,
        Value = r
    }), "Value", "Text", account.Role);

    System.Diagnostics.Debug.WriteLine("Trả về view với model.");
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