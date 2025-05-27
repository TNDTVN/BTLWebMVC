using System;
using System.Linq;
using System.Web.Mvc;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;
using System.Data.Entity;
using System.Diagnostics;

namespace BTLWebMVC.Controllers.Manager
{
    public class CategoryController : Controller
    {
        private Context db = new Context();

        // GET: Manager/Category
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing.");
                return RedirectToAction("Index", "Home");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                Debug.WriteLine($"Account with ID {parsedAccountId} not found or not Admin.");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CurrentPage = "Category";

            // Phân trang
            var totalRecords = db.Categories.Count();
            var categories = db.Categories
                .OrderBy(c => c.CategoryID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalRecords = totalRecords;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            ViewBag.IsFirstPage = page == 1;
            ViewBag.IsLastPage = page >= ViewBag.TotalPages;

            Debug.WriteLine($"Categories Count: {totalRecords}, Page: {page}, PageSize: {pageSize}, TotalPages: {ViewBag.TotalPages}, IsFirstPage: {ViewBag.IsFirstPage}, IsLastPage: {ViewBag.IsLastPage}");
            return View(categories);
        }

        // GET: Manager/Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // POST: Manager/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryName,Description")] Category category)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            if (ModelState.IsValid)
            {
                if (db.Categories.Any(c => c.CategoryName == category.CategoryName))
                {
                    return Json(new { success = false, message = "Tên loại sản phẩm đã tồn tại!" });
                }

                db.Categories.Add(category);
                db.SaveChanges();
                Debug.WriteLine($"Category Created: ID={category.CategoryID}, Name={category.CategoryName}");
                return Json(new { success = true, message = "Thêm loại sản phẩm thành công!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
        }

        // GET: Manager/Category/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" }, JsonRequestBehavior.AllowGet);

            var category = db.Categories.Find(id);
            if (category == null)
            {
                Debug.WriteLine($"Category with ID {id} not found.");
                return Json(new { success = false, message = "Loại sản phẩm không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    category.CategoryID,
                    category.CategoryName,
                    category.Description
                }
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Manager/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description")] Category category)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            if (ModelState.IsValid)
            {
                var existingCategory = db.Categories.Find(category.CategoryID);
                if (existingCategory == null)
                {
                    Debug.WriteLine($"Category with ID {category.CategoryID} not found.");
                    return Json(new { success = false, message = "Loại sản phẩm không tồn tại!" });
                }

                if (db.Categories.Any(c => c.CategoryName == category.CategoryName && c.CategoryID != category.CategoryID))
                {
                    return Json(new { success = false, message = "Tên loại sản phẩm đã tồn tại!" });
                }

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                db.Entry(existingCategory).State = EntityState.Modified;
                db.SaveChanges();
                Debug.WriteLine($"Category Updated: ID={category.CategoryID}, Name={category.CategoryName}");
                return Json(new { success = true, message = "Cập nhật loại sản phẩm thành công!" });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Dữ liệu không hợp lệ: " + string.Join("; ", errors) });
        }

        // POST: Manager/Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" });

            var category = db.Categories.Include(c => c.Products).FirstOrDefault(c => c.CategoryID == id);
            if (category == null)
            {
                Debug.WriteLine($"Category with ID {id} not found.");
                return Json(new { success = false, message = "Loại sản phẩm không tồn tại!" });
            }

            if (category.Products.Any())
            {
                Debug.WriteLine($"Category ID {id} has {category.Products.Count} products, cannot delete.");
                return Json(new { success = false, message = "Không thể xóa vì loại sản phẩm này đang chứa sản phẩm!" });
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            Debug.WriteLine($"Category Deleted: ID={id}");
            return Json(new { success = true, message = "Xóa loại sản phẩm thành công!" });
        }

        // GET: Manager/Category/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            if (!IsAdmin()) return Json(new { success = false, message = "Không có quyền!" }, JsonRequestBehavior.AllowGet);

            var category = db.Categories.Find(id);
            if (category == null)
            {
                Debug.WriteLine($"Category with ID {id} not found.");
                return Json(new { success = false, message = "Loại sản phẩm không tồn tại!" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                success = true,
                data = new
                {
                    category.CategoryID,
                    category.CategoryName,
                    category.Description
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