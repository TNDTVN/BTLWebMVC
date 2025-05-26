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
                return RedirectToAction("Login", "Login");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return RedirectToAction("Login", "Login");
            }

            ViewBag.UserRole = account.Role;
            if (account.Role != "Admin" && account.Role != "Employee")
            {
                return RedirectToAction("Index", "Home");
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
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,ContactName,Address,City,PostalCode,Country,Phone,Email,AccountID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Thêm khách hàng thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo khách hàng: " + ex.Message);
                }
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID);
            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }

        // GET: Customers/Edit
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentPage = "Customers";
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID);
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
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật khách hàng thành công!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật khách hàng: " + ex.Message);
                }
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID);
            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }

        // POST: Customers/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")] // Giả sử bạn có CustomAuthorize để kiểm tra quyền Admin
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