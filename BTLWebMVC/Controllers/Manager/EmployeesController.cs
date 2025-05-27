using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;
using PagedList;
using System.IO;
using System.Web;

namespace BTLWebMVC.Controllers.Manager
{
    public class EmployeesController : Controller
    {
        private Context db = new Context();

        [HttpGet]
        public ActionResult Index(int? page)
        {

           
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int id))
            {
                Console.WriteLine("Session AccountId invalid or missing.");
                TempData["ErrorMessage"] = "Vui lòng đăng nhập";
                return RedirectToAction("Index", "Home");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                Console.WriteLine($"Account with ID {id} not found.");
                TempData["ErrorMessage"] = "Vui lòng đăng nhập";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.UserRole = account.Role;
            if (account.Role != "Admin" && account.Role != "Employee")
            {
                Console.WriteLine($"Account ID {id} role {account.Role} access denied.");
                TempData["ErrorMessage"] = "Vui lòng đăng nhập";
                return RedirectToAction("Index", "Home");
            }
      

            ViewBag.CurrentPage = "Employees";

            
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var employees = db.Employees
                .Include(e => e.Account)
                .OrderBy(e => e.EmployeeID)
                .ToPagedList(pageNumber, pageSize);

            Console.WriteLine($"Employees Count: {employees.TotalItemCount}, Page: {pageNumber}, PageSize: {pageSize}, TotalPages: {employees.PageCount}");
            return View(employees);
        }

        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Employees";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,BirthDate,HireDate,Address,City,PostalCode,Country,Phone,Email")] Employee employee, string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra Username độc nhất
                    if (db.Accounts.Any(a => a.Username == Username))
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                        return View(employee);
                    }

                    // Tạo tài khoản mới
                    var account = new Account
                    {
                        Username = Username,
                        Password = Password, // Lưu ý: Nên mã hóa mật khẩu trong thực tế
                        Email = employee.Email,
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Employee",
                        IsLock = false
                    };
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    // Gán AccountID cho Employee
                    employee.AccountID = account.AccountID;
                    db.Employees.Add(employee);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm nhân viên và tài khoản thành công!";
                    Console.WriteLine($"Employee Created: ID={employee.EmployeeID}, Name={employee.FirstName} {employee.LastName}, AccountID={account.AccountID}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating employee: {ex.Message}");
                    TempData["ErrorMessage"] = "Lỗi khi thêm nhân viên: " + ex.Message;
                }
            }

            return View(employee);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Edit(int? id)
        {
            ViewBag.CurrentPage = "Employees";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không có ID nhân viên!";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Include(e => e.Account).FirstOrDefault(e => e.EmployeeID == id);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên!";
                return RedirectToAction("Index", "Employees");
            }
            if (employee.Account == null)
            {
                ViewBag.DebugMessage = $"Error: Account is null for EmployeeID={id}";
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                return RedirectToAction("Index");
            }
            // Kiểm tra vai trò hợp lệ
            if (employee.Account.Role != "Admin" && employee.Account.Role != "Employee")
            {
                ViewBag.DebugMessage = $"Error: Invalid Role '{employee.Account.Role}' for EmployeeID={id}";
                TempData["ErrorMessage"] = "Vai trò không hợp lệ trong cơ sở dữ liệu!";
                return RedirectToAction("Index");
            }
            ViewBag.DebugMessage = $"Employee Edit: ID={id}, Name={employee.FirstName} {employee.LastName}, Role={employee.Account.Role}";

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,BirthDate,HireDate,Address,City,PostalCode,Country,Phone,Email,AccountID")] Employee employee, string Role)
        {
            if (ModelState.IsValid)
            {
                if (Role != "Admin" && Role != "Employee")
                {
                    ModelState.AddModelError("Role", "Vai trò không hợp lệ!");
                    return View(employee);
                }
                try
                {
                    // Cập nhật Employee
                    db.Entry(employee).State = EntityState.Modified;

                    // Cập nhật Role của Account
                    var account = db.Accounts.Find(employee.AccountID);
                    if (account == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                        return View(employee);
                    }
                    account.Role = Role;
                    db.Entry(account).State = EntityState.Modified;

                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật nhân viên thành công!";
                    Console.WriteLine($"Employee Updated: ID={employee.EmployeeID}, Name={employee.FirstName} {employee.LastName}, Role={Role}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating employee: {ex.Message}");
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật nhân viên: " + ex.Message;
                }
            }

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                Employee employee = db.Employees.Include(e => e.Account).FirstOrDefault(e => e.EmployeeID == id);
                if (employee == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });
                }
                if (employee.Account != null && employee.Account.Role == "Admin")
                {
                    return Json(new { success = false, message = "Không thể xóa nhân viên vì đây là tài khoản quản lý!" });
                }
                bool hasOrders = db.Orders.Any(o => o.EmployeeID == id);
                if (hasOrders)
                {
                    return Json(new { success = false, message = "Không thể xóa nhân viên vì đã liên kết với đơn hàng!" });
                }

                // Xóa ảnh đại diện nếu không phải profile.jpg
                if (employee.Account != null && !string.IsNullOrEmpty(employee.Account.ProfileImage) && employee.Account.ProfileImage != "profile.jpg")
                {
                    string imagePath = Server.MapPath($"~/Content/accountImages/{employee.Account.ProfileImage}");
                    if (System.IO.File.Exists(imagePath))
                    {
                        try
                        {
                            System.IO.File.Delete(imagePath);
                            Console.WriteLine($"Profile image deleted: {employee.Account.ProfileImage}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting profile image {employee.Account.ProfileImage}: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Profile image not found: {imagePath}");
                    }
                }

                // Xóa Account
                if (employee.Account != null)
                {
                    db.Accounts.Remove(employee.Account);
                }
                // Xóa Employee
                db.Employees.Remove(employee);
                db.SaveChanges();

                Console.WriteLine($"Employee and Account Deleted: EmployeeID={id}, AccountID={employee.AccountID}");
                return Json(new { success = true, message = "Xóa nhân viên và tài khoản thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting employee ID={id}: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Lỗi khi xóa nhân viên: {ex.Message}" });
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Không có ID nhân viên!" }, JsonRequestBehavior.AllowGet);
            }
            var employee = db.Employees.Include(e => e.Account).FirstOrDefault(e => e.EmployeeID == id);
            if (employee == null)
            {
                Console.WriteLine($"Employee with ID {id} not found.");
                return Json(new { success = false, message = "Không tìm thấy nhân viên!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    employee.EmployeeID,
                    FullName = $"{employee.FirstName} {employee.LastName}",
                    BirthDate = employee.BirthDate.ToString("yyyy-MM-dd"),
                    HireDate = employee.HireDate.ToString("yyyy-MM-dd"),
                    employee.City,
                    employee.PostalCode,
                    employee.Country,
                    employee.Phone,
                    employee.Email,
                    AccountUsername = employee.Account?.Username,
                    HasOrders = db.Orders.Any(o => o.EmployeeID == employee.EmployeeID),
                    IsAdmin = employee.Account?.Role == "Admin"
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult EditProfile()
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int id))
            {
                Console.WriteLine("Session AccountId invalid or missing.");
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để chỉnh sửa thông tin!";
                return RedirectToAction("Index", "Home");
            }

            var employee = db.Employees.Include(e => e.Account)
                .FirstOrDefault(e => e.AccountID == id);
            if (employee == null || employee.Account == null)
            {
                Console.WriteLine($"Employee or Account not found for AccountID={id}.");
                TempData["ErrorMessage"] = "Không tìm thấy thông tin nhân viên!";
                return RedirectToAction("Index", "HomeManager");
            }

            if (employee.Account.Role != "Admin" && employee.Account.Role != "Employee")
            {
                Console.WriteLine($"Account ID {id} role {employee.Account.Role} access denied.");
                TempData["ErrorMessage"] = "Bạn không có quyền truy cập!";
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.CurrentPage = "EditProfile";
            ViewBag.ProfileImage = employee.Account.ProfileImage ?? "profile.jpg";
            Console.WriteLine($"Edit Profile: AccountID={id}, Name={employee.FirstName} {employee.LastName}");
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "EmployeeID,FirstName,LastName,BirthDate,Address,City,PostalCode,Country,Phone,Email,AccountID,HireDate")] Employee employee, HttpPostedFileBase ProfileImage)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int id))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để chỉnh sửa thông tin!";
                return RedirectToAction("Index", "Home");
            }

            if (employee.AccountID != id)
            {
                TempData["ErrorMessage"] = "Bạn chỉ có thể chỉnh sửa thông tin của chính mình!";
                return RedirectToAction("Index", "HomeManager");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEmployee = db.Employees.Include(e => e.Account)
                        .FirstOrDefault(e => e.EmployeeID == employee.EmployeeID && e.AccountID == id);
                    if (existingEmployee == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy thông tin nhân viên!";
                        return RedirectToAction("Index", "HomeManager");
                    }

                    // Cập nhật thông tin Employee
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.BirthDate = employee.BirthDate;
                    // existingEmployee.HireDate = employee.HireDate;
                    existingEmployee.Address = employee.Address;
                    existingEmployee.City = employee.City;
                    existingEmployee.PostalCode = employee.PostalCode;
                    existingEmployee.Country = employee.Country;
                    existingEmployee.Phone = employee.Phone;
                    existingEmployee.Email = employee.Email;

                    // Cập nhật Email trong Account
                    existingEmployee.Account.Email = employee.Email;

                    // Xử lý ảnh đại diện
                    string oldImage = existingEmployee.Account.ProfileImage;
                    if (ProfileImage != null && ProfileImage.ContentLength > 0)
                    {
                        // Kiểm tra định dạng ảnh
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                        string extension = Path.GetExtension(ProfileImage.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ProfileImage", "Chỉ chấp nhận tệp .jpg, .jpeg hoặc .png!");
                            ViewBag.ProfileImage = oldImage;
                            return View(employee);
                        }

                        // Kiểm tra kích thước ảnh
                        if (ProfileImage.ContentLength > 5 * 1024 * 1024) // 5MB
                        {
                            ModelState.AddModelError("ProfileImage", "Tệp ảnh không được vượt quá 5MB!");
                            ViewBag.ProfileImage = oldImage;
                            return View(employee);
                        }

                        // Tạo tên tệp duy nhất
                        string fileName = $"{Guid.NewGuid()}{extension}";
                        string path = Path.Combine(Server.MapPath("~/Content/accountImages"), fileName);
                        ProfileImage.SaveAs(path);

                        // Xóa ảnh cũ nếu không phải profile.jpg
                        if (!string.IsNullOrEmpty(oldImage) && oldImage != "profile.jpg")
                        {
                            string oldImagePath = Server.MapPath($"~/Content/accountImages/{oldImage}");
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                try
                                {
                                    System.IO.File.Delete(oldImagePath);
                                    Console.WriteLine($"Old profile image deleted: {oldImage}");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error deleting old profile image {oldImage}: {ex.Message}");
                                }
                            }
                        }

                        // Cập nhật ProfileImage
                        existingEmployee.Account.ProfileImage = fileName;
                        Console.WriteLine($"New profile image saved: {fileName}");
                    }

                    db.Entry(existingEmployee).State = EntityState.Modified;
                    db.Entry(existingEmployee.Account).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật thông tin cá nhân thành công!";
                    Console.WriteLine($"Profile Updated: EmployeeID={employee.EmployeeID}, Name={employee.FirstName} {employee.LastName}");
                    return RedirectToAction("EditProfile");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating profile: {ex.Message}");
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật thông tin: " + ex.Message;
                }
            }

            ViewBag.ProfileImage = db.Accounts.Find(employee.AccountID)?.ProfileImage ?? "profile.jpg";
            ViewBag.CurrentPage = "EditProfile";
            return View(employee);
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