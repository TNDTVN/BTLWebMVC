using System;
using System.Linq;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;
using System.Data.Entity;
using System.Diagnostics;

namespace BTLWebMVC.Controllers.Manager
{
    public class SupplierController : Controller
    {
        private Context db = new Context();

        // GET: Manager/Supplier
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing.");
                return RedirectToAction("Index", "Home");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null || account.Role != "Admin")
            {
                Debug.WriteLine($"Account with ID {parsedAccountId} not found or not Admin.");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CurrentPage = "Supplier";

            // Phân trang
            var totalRecords = db.Suppliers.Count();
            var suppliers = db.Suppliers
                .OrderBy(s => s.SupplierID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page >= ViewBag.TotalPages;

            Debug.WriteLine($"Suppliers Count: {totalRecords}, Page: {page}, PageSize: {pageSize}, TotalPages: {ViewBag.TotalPages}, IsFirstPage: {ViewBag.IsFirstPage}, IsLastPage: {ViewBag.IsLastPage}");
            return View(suppliers);
        }

        // GET: Manager/Supplier/Create
        [HttpGet]
        public ActionResult Create()
        {
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // POST: Manager/Supplier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupplierName,ContactName,Address,City,PostalCode,Country,Phone,Email")] Supplier supplier)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            if (ModelState.IsValid)
            {
                if (db.Suppliers.Any(s => s.SupplierName == supplier.SupplierName))
                {
                    return Json(new { success = false, message = "Tên nhà cung cấp đã tồn tại!" });
                }

                db.Suppliers.Add(supplier);
                db.SaveChanges();
                Debug.WriteLine($"Supplier Created: ID={supplier.SupplierID}, Name={supplier.SupplierName}");
                return Json(new { success = true, message = "Thêm nhà cung cấp thành công!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
        }

        // GET: Manager/Supplier/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" }, JsonRequestBehavior.AllowGet);

            var supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                Debug.WriteLine($"Supplier with ID {id} not found.");
                return Json(new { success = false, message = "Nhà cung cấp không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    supplier.SupplierID,
                    supplier.SupplierName,
                    supplier.ContactName,
                    supplier.Address,
                    supplier.City,
                    supplier.PostalCode,
                    supplier.Country,
                    supplier.Phone,
                    supplier.Email
                }
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Manager/Supplier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SupplierID,SupplierName,ContactName,Address,City,PostalCode,Country,Phone,Email")] Supplier supplier)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            if (ModelState.IsValid)
            {
                var existingSupplier = db.Suppliers.Find(supplier.SupplierID);
                if (existingSupplier == null)
                {
                    Debug.WriteLine($"Supplier with ID {supplier.SupplierID} not found.");
                    return Json(new { success = false, message = "Nhà cung cấp không tồn tại!" });
                }

                if (db.Suppliers.Any(s => s.SupplierName == supplier.SupplierName && s.SupplierID != supplier.SupplierID))
                {
                    return Json(new { success = false, message = "Tên nhà cung cấp đã tồn tại!" });
                }

                existingSupplier.SupplierName = supplier.SupplierName;
                existingSupplier.ContactName = supplier.ContactName;
                existingSupplier.Address = supplier.Address;
                existingSupplier.City = supplier.City;
                existingSupplier.PostalCode = supplier.PostalCode;
                existingSupplier.Country = supplier.Country;
                existingSupplier.Phone = supplier.Phone;
                existingSupplier.Email = supplier.Email;
                db.Entry(existingSupplier).State = EntityState.Modified;
                db.SaveChanges();
                Debug.WriteLine($"Supplier Updated: ID={supplier.SupplierID}, Name={supplier.SupplierName}");
                return Json(new { success = true, message = "Cập nhật nhà cung cấp thành công!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
        }

        // POST: Manager/Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            var supplier = db.Suppliers.Include(s => s.Products).FirstOrDefault(s => s.SupplierID == id);
            if (supplier == null)
            {
                Debug.WriteLine($"Supplier with ID {id} not found.");
                return Json(new { success = false, message = "Nhà cung cấp không tồn tại!" });
            }

            if (supplier.Products.Any())
            {
                Debug.WriteLine($"Supplier ID {id} has {supplier.Products.Count} products, cannot delete.");
                return Json(new { success = false, message = "Không thể xóa vì nhà cung cấp này đang liên kết với sản phẩm!" });
            }

            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            Debug.WriteLine($"Supplier Deleted: ID={id}");
            return Json(new { success = true, message = "Xóa nhà cung cấp thành công!" });
        }

        // GET: Manager/Supplier/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" }, JsonRequestBehavior.AllowGet);

            var supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                Debug.WriteLine($"Supplier with ID {id} not found.");
                return Json(new { success = false, message = "Nhà cung cấp không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    supplier.SupplierID,
                    supplier.SupplierName,
                    supplier.ContactName,
                    supplier.Address,
                    supplier.City,
                    supplier.PostalCode,
                    supplier.Country,
                    supplier.Phone,
                    supplier.Email
                }
            }, JsonRequestBehavior.AllowGet);
        }

        private bool IsAdmin()
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing.");
                return false;
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            return account != null && account.Role == "Admin";
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