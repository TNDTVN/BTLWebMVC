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
using PagedList;
namespace BTLWebMVC.Controllers.Manager
{
    public class AccountsController : Controller
    {
        private Context db = new Context();


        // GET: Accounts
        public ActionResult Index(int?page)
        {
            ViewBag.CurrentPage = "Accounts";
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var accounts = db.Accounts.Include(a => a.Employees).Include(a => a.Customers).OrderBy(a => a.AccountID).ToPagedList(pageNumber, pageSize);
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
                if (account.IsLock == (isLock == 1))
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
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                return RedirectToAction("Index");
            }

            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                return RedirectToAction("Index");
            }

            List<string> allowedRoles;
            if (account.Role == "Customer")
            {
                allowedRoles = new List<string> { "Customer" };
            }
            else if (account.Role == "Employee" || account.Role == "Admin")
            {
                allowedRoles = new List<string> { "Employee", "Admin" };
            }
            else
            {
                allowedRoles = new List<string> { account.Role };
            }

            ViewBag.Role = new SelectList(allowedRoles.Select(r => new SelectListItem
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
            var result = new { success = false, errors = new Dictionary<string, string>() };

            if (ModelState.IsValid)
            {
                var existingAccount = db.Accounts.Find(account.AccountID);
                if (existingAccount == null)
                {
                    result.errors.Add("", "Không tìm thấy tài khoản với AccountID: " + account.AccountID);
                    return Json(result);
                }

                // Kiểm tra email trùng (trừ email của chính tài khoản đang chỉnh sửa)
                if (db.Accounts.Any(a => a.Email == account.Email && a.AccountID != account.AccountID))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng bởi tài khoản khác.");
                }

                // Kiểm tra vai trò hợp lệ
                if (existingAccount.Role == "Customer" && account.Role != "Customer")
                {
                    ModelState.AddModelError("Role", "Không thể thay đổi vai trò của tài khoản Customer.");
                }
                else if ((existingAccount.Role == "Employee" || existingAccount.Role == "Admin") && account.Role == "Customer")
                {
                    ModelState.AddModelError("Role", "Không thể thay đổi vai trò sang Customer.");
                }

                if (!ModelState.IsValid)
                {
                    // Thu thập lỗi validation
                    foreach (var error in ModelState)
                    {
                        foreach (var err in error.Value.Errors)
                        {
                            result.errors.Add(error.Key, err.ErrorMessage);
                        }
                    }
                    return Json(result);
                }

                // Cập nhật thông tin tài khoản
                existingAccount.Username = account.Username;
                existingAccount.Password = account.Password;
                existingAccount.Email = account.Email;
                existingAccount.Role = account.Role;
                existingAccount.IsLock = account.IsLock;

                // Đồng bộ email với bảng Customer hoặc Employee
                if (existingAccount.Role == "Customer")
                {
                    var customer = db.Customers.FirstOrDefault(c => c.AccountID == account.AccountID);
                    if (customer != null)
                    {
                        customer.Email = account.Email;
                        db.Entry(customer).State = EntityState.Modified;
                    }
                }
                else if (existingAccount.Role == "Employee" || existingAccount.Role == "Admin")
                {
                    var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
                    if (employee != null)
                    {
                        employee.Email = account.Email;
                        db.Entry(employee).State = EntityState.Modified;
                    }
                }

                // Giữ nguyên ảnh cũ nếu không có ảnh mới
                if (FileAnh != null && FileAnh.ContentLength > 0)
                {
                    string oldImagePath = existingAccount.ProfileImage;
                    string fileExtension = Path.GetExtension(FileAnh.FileName).ToLower();

                    // Kiểm tra định dạng file
                    if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(fileExtension))
                    {
                        result.errors.Add("FileAnh", "Chỉ hỗ trợ file ảnh .jpg, .jpeg, .png, .gif");
                        return Json(result);
                    }

                    // Tạo tên file mới
                    string fileName = Guid.NewGuid().ToString() + fileExtension;
                    string path = Path.Combine(Server.MapPath("~/Content/accountImages"), fileName);
                    string fileNameOnly = fileName;

                    // Kiểm tra độ dài tên file
                    if (fileNameOnly.Length > 255)
                    {
                        result.errors.Add("FileAnh", "Tên file quá dài, vui lòng chọn file có tên ngắn hơn.");
                        return Json(result);
                    }

                    // Đảm bảo thư mục tồn tại
                    string directory = Server.MapPath("~/Content/accountImages");
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Lưu file
                    try
                    {
                        FileAnh.SaveAs(path);
                        existingAccount.ProfileImage = fileNameOnly;

                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(Server.MapPath("~/Content/accountImages/" + oldImagePath)))
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/accountImages/" + oldImagePath));
                        }
                    }
                    catch (Exception ex)
                    {
                        result.errors.Add("FileAnh", "Lỗi khi lưu file ảnh: " + ex.Message);
                        return Json(result);
                    }
                }
                else
                {
                    existingAccount.ProfileImage = existingAccount.ProfileImage; // Giữ nguyên ảnh cũ
                }

                // Cập nhật CSDL
                db.Entry(existingAccount).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    result.errors.Add("", "Lỗi khi lưu vào CSDL: " + ex.Message);
                    return Json(result);
                }
            }
            else
            {
                // Thu thập lỗi validation
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        result.errors.Add(error.Key, err.ErrorMessage);
                    }
                }
                return Json(result);
            }
        }


        // them tai khoan
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Accounts";

            // Lấy danh sách vai trò duy nhất từ CSDL
            var roles = db.Accounts.Select(a => a.Role).Distinct().ToList();
            ViewBag.Role = new SelectList(roles.Select(r => new SelectListItem
            {
                Text = r,
                Value = r
            }), "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Email,Role,IsLock")] Account account, string CustomerName, string FirstName, string LastName, DateTime? BirthDate, DateTime? HireDate)
        {
            var result = new { success = false, errors = new Dictionary<string, string>() };

            if (ModelState.IsValid)
            {
                // Kiểm tra username trùng
                if (db.Accounts.Any(a => a.Username == account.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã được sử dụng.");
                }

                // Kiểm tra email trùng
                if (db.Accounts.Any(a => a.Email == account.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng bởi tài khoản khác.");
                }

                // Kiểm tra các trường bắt buộc cho Employee/Admin
                if (account.Role == "Employee" || account.Role == "Admin")
                {
                    if (string.IsNullOrEmpty(FirstName))
                        ModelState.AddModelError("FirstName", "Họ là bắt buộc.");
                    if (string.IsNullOrEmpty(LastName))
                        ModelState.AddModelError("LastName", "Tên là bắt buộc.");
                    if (!BirthDate.HasValue)
                        ModelState.AddModelError("BirthDate", "Ngày sinh là bắt buộc.");
                    if (!HireDate.HasValue)
                        ModelState.AddModelError("HireDate", "Ngày thuê là bắt buộc.");
                }

                // Kiểm tra trường bắt buộc cho Customer
                if (account.Role == "Customer" && string.IsNullOrEmpty(CustomerName))
                {
                    ModelState.AddModelError("CustomerName", "Tên khách hàng là bắt buộc.");
                }

                if (!ModelState.IsValid)
                {
                    foreach (var error in ModelState)
                    {
                        foreach (var err in error.Value.Errors)
                        {
                            result.errors.Add(error.Key, err.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Lỗi ModelState: Key={error.Key}, Error={err.ErrorMessage}");
                        }
                    }
                    return Json(result);
                }

                // Gán giá trị mặc định cho tài khoản
                account.CreatedDate = DateTime.Now;
                account.ProfileImage = "profile.jpg"; // Ảnh đại diện mặc định
                account.TokenCode = null; // Không sử dụng TokenCode

                // Thêm tài khoản vào CSDL
                db.Accounts.Add(account);
                try
                {
                    db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine($"Tài khoản được tạo: AccountID={account.AccountID}, Role={account.Role}");

                    // Tạo bản ghi trong bảng Customer hoặc Employee dựa trên Role
                    try
                    {
                        if (account.Role == "Customer")
                        {
                            var customer = new Customer
                            {
                                AccountID = account.AccountID,
                                Email = account.Email,
                                CustomerName = CustomerName,
                                ContactName = CustomerName,
                                Address = "",
                                City = "",
                                PostalCode = "",
                                Country = ""
                            };
                            db.Customers.Add(customer);
                            System.Diagnostics.Debug.WriteLine($"Tạo bản ghi Customer: CustomerName={CustomerName}, AccountID={account.AccountID}");
                        }
                        else if (account.Role == "Employee" || account.Role == "Admin")
                        {
                            var employee = new Employee
                            {
                                AccountID = account.AccountID,
                                Email = account.Email,
                                FirstName = FirstName,
                                LastName = LastName,
                                BirthDate = BirthDate.Value,
                                HireDate = HireDate.Value,
                                Address = "",
                                City = "",
                                PostalCode = "",
                                Country = ""
                            };
                            db.Employees.Add(employee);
                            System.Diagnostics.Debug.WriteLine($"Tạo bản ghi Employee: FirstName={FirstName}, LastName={LastName}, AccountID={account.AccountID}");
                        }

                        // Lưu các thay đổi vào CSDL
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Lưu thay đổi vào CSDL thành công.");
                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {
                        // Xóa tài khoản đã tạo nếu thất bại
                        db.Accounts.Remove(account);
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine($"Đã xóa tài khoản AccountID={account.AccountID} do lỗi tạo Customer/Employee.");

                        // Ghi lỗi
                        result.errors.Add("", "Lỗi khi tạo Customer/Employee: " + ex.Message);
                        System.Diagnostics.Debug.WriteLine($"Lỗi khi tạo Customer/Employee: {ex.Message}, StackTrace: {ex.StackTrace}");
                        return Json(result);
                    }
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            result.errors.Add(validationError.PropertyName, validationError.ErrorMessage);
                            System.Diagnostics.Debug.WriteLine($"Lỗi xác thực: Property={validationError.PropertyName}, Error={validationError.ErrorMessage}");
                        }
                    }
                    return Json(result);
                }
                catch (Exception ex)
                {
                    result.errors.Add("", "Lỗi khi lưu vào CSDL: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine($"Lỗi chung: {ex.Message}, StackTrace: {ex.StackTrace}");
                    return Json(result);
                }
            }
            else
            {
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        result.errors.Add(error.Key, err.ErrorMessage);
                        System.Diagnostics.Debug.WriteLine($"Lỗi ModelState: Key={error.Key}, Error={err.ErrorMessage}");
                    }
                }
                return Json(result);
            }
        }

        // xem chi tiet tai khona

        [HttpGet]
        public ActionResult Detail(int? id)
        {
            ViewBag.CurrentPage = "Accounts";
            if (id == null)
            {
                System.Diagnostics.Debug.WriteLine("Không tìm thấy ID tài khoản");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            Account account = db.Accounts.Include(a => a.Employees).Include(a => a.Customers).FirstOrDefault(a => a.AccountID == id);


            if (account == null)
            {
                System.Diagnostics.Debug.WriteLine($"Không tìm thấy tài khoản với ID: {id}");
                return HttpNotFound();
            }

            System.Diagnostics.Debug.WriteLine($"Tải chi tiết tài khoản thành công: AccountID={account.AccountID}, Username={account.Username}");
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