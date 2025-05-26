using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;
using PagedList;

namespace BTLWebMVC.Controllers.Manager
{
    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index(int? page)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int id))
            {
                Console.WriteLine("Session AccountId invalid or missing.");
                return RedirectToAction("Login", "Login");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                Console.WriteLine($"Account with ID {id} not found.");
                return RedirectToAction("Login", "Login");
            }

            ViewBag.UserRole = account.Role;
            if (account.Role != "Admin" && account.Role != "Employee")
            {
                Console.WriteLine($"Account ID {id} role {account.Role} access denied.");
                return RedirectToAction("AccessDenied", "Home");
            }

            ViewBag.CurrentPage = "Customers";
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            var customers = db.Customers.Include(c => c.Account).OrderBy(c => c.AccountID).ToPagedList(pageNumber, pageSize);
            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Customers";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,ContactName,Address,City,PostalCode,Country,Phone,Email")] Customer customer, string Username, string Password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra Username độc nhất
                    if (db.Accounts.Any(a => a.Username == Username))
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                        return View(customer);
                    }

                    // Tạo tài khoản mới
                    var account = new Account
                    {
                        Username = Username,
                        Password = Password, // Lưu ý: Nên mã hóa mật khẩu trong thực tế
                        Email = customer.Email,
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Customer",
                        IsLock = false
                    };
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    // Gán AccountID cho Customer
                    customer.AccountID = account.AccountID;
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm khách hàng và tài khoản thành công!";
                    Console.WriteLine($"Customer Created: ID={customer.CustomerID}, Name={customer.CustomerName}, AccountID={account.AccountID}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating customer: {ex.Message}");
                    TempData["ErrorMessage"] = "Lỗi khi thêm khách hàng: " + ex.Message;
                }
            }

            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }

        // GET: Customers/Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.CurrentPage = "Customers";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không có ID khách hàng!";
                return RedirectToAction("Index");
            }

            Customer customer = db.Customers.Include(c => c.Account).FirstOrDefault(c => c.CustomerID == id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng!";
                return RedirectToAction("Index");
            }
            if (customer.Account == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                return RedirectToAction("Index");
            }

            ViewBag.DebugMessage = $"Customer Edit: ID={id}, Name={customer.CustomerName}, Role={customer.Account.Role}";
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,ContactName,Address,City,PostalCode,Country,Phone,Email,AccountID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = db.Customers.Include(c => c.Account).FirstOrDefault(c => c.CustomerID == customer.CustomerID);
                    if (existingCustomer == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy khách hàng!";
                        return RedirectToAction("Index");
                    }
                    if (existingCustomer.Account == null)
                    {
                        TempData["ErrorMessage"] = "Không tìm thấy tài khoản liên kết!";
                        return RedirectToAction("Index");
                    }

                    // Cập nhật thông tin Customer
                    existingCustomer.CustomerName = customer.CustomerName;
                    existingCustomer.ContactName = customer.ContactName;
                    existingCustomer.Address = customer.Address;
                    existingCustomer.City = customer.City;
                    existingCustomer.PostalCode = customer.PostalCode;
                    existingCustomer.Country = customer.Country;
                    existingCustomer.Phone = customer.Phone;
                    existingCustomer.Email = customer.Email;

                    // Cập nhật Email trong Account
                    existingCustomer.Account.Email = customer.Email;

                    db.Entry(existingCustomer).State = EntityState.Modified;
                    db.Entry(existingCustomer.Account).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                    Console.WriteLine($"Customer Updated: ID={customer.CustomerID}, Name={customer.CustomerName}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating customer: {ex.Message}");
                    TempData["ErrorMessage"] = "Lỗi khi cập nhật khách hàng: " + ex.Message;
                }
            }

            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }

        // POST: Customers/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                Customer customer = db.Customers.Include(c => c.Account).FirstOrDefault(c => c.CustomerID == id);
                if (customer == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy khách hàng!" });
                }

                // Kiểm tra xem khách hàng có đơn hàng hay không
                bool hasOrders = db.Orders.Any(o => o.CustomerID == id);
                if (hasOrders)
                {
                    return Json(new { success = false, message = "Không thể xóa khách hàng vì đã liên kết với đơn hàng!" });
                }

                // Xóa Account liên kết (nếu có)
                if (customer.Account != null)
                {
                    db.Accounts.Remove(customer.Account);
                }

                // Xóa Customer
                db.Customers.Remove(customer);
                db.SaveChanges();

                Console.WriteLine($"Customer and Account Deleted: CustomerID={id}, AccountID={customer.AccountID}");
                return Json(new { success = true, message = "Xóa khách hàng thành công!" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting customer ID={id}: {ex.Message}\nStackTrace: {ex.StackTrace}");
                return Json(new { success = false, message = $"Lỗi khi xóa khách hàng: {ex.Message}" });
            }
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