using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;
using PagedList;
using System.Diagnostics;

namespace BTLWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(int? page)
        {
            ViewBag.CurrentPage = "Products";
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var products = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .OrderBy(p => p.ProductID)
                .ToPagedList(pageNumber, pageSize);

            Debug.WriteLine($"Products Count: {products.TotalItemCount}, Page: {pageNumber}, PageSize: {pageSize}, TotalPages: {products.PageCount}");
            return View(products);
        }

        public ActionResult Details(int? id)
        {
            ViewBag.CurrentPage = "Products";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không có ID sản phẩm!";
                return RedirectToAction("Index");
            }
            Product product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction("Index");
            }
            Debug.WriteLine($"Product Details: ID={id}, Name={product.ProductName}");
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Products";
            Product product = new Product();
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,CategoryID,SupplierID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductDescription")] Product product, HttpPostedFileBase[] ImageFiles)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();

                    if (ImageFiles != null && ImageFiles.Any(f => f != null && f.ContentLength > 0))
                    {
                        string thuMuc = Server.MapPath("~/Content/Images/");
                        if (!Directory.Exists(thuMuc))
                        {
                            Directory.CreateDirectory(thuMuc);
                        }

                        foreach (var file in ImageFiles.Where(f => f != null && f.ContentLength > 0))
                        {
                            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                            string extension = Path.GetExtension(file.FileName).ToLower();
                            if (!allowedExtensions.Contains(extension))
                            {
                                ModelState.AddModelError("ImageFiles", "Chỉ chấp nhận tệp .jpg, .jpeg hoặc .png!");
                                continue;
                            }
                            if (file.ContentLength > 5 * 1024 * 1024) // 5MB
                            {
                                ModelState.AddModelError("ImageFiles", "Tệp ảnh không được vượt quá 5MB!");
                                continue;
                            }

                            string tenFileGoc = Path.GetFileNameWithoutExtension(file.FileName);
                            string tenFile = $"{tenFileGoc}_{DateTime.Now.Ticks}{extension}";
                            string duongDanAnh = Path.Combine(thuMuc, tenFile);
                            file.SaveAs(duongDanAnh);

                            db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                        }
                        db.SaveChanges();
                    }

                    if (ModelState.IsValid)
                    {
                        TempData["SuccessMessage"] = "Tạo sản phẩm thành công!";
                        Debug.WriteLine($"Product Created: ID={product.ProductID}, Name={product.ProductName}");
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating product: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu sản phẩm: " + ex.Message);
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.CurrentPage = "Products";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Không có ID sản phẩm!";
                return RedirectToAction("Index");
            }
            Product product = db.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            Debug.WriteLine($"Product Edit: ID={id}, Name={product.ProductName}, Images.Count={product.Images?.Count ?? 0}");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,CategoryID,SupplierID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductDescription")] Product product, HttpPostedFileBase[] ImageFiles, int[] ImagesToDelete)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    if (ImagesToDelete != null && ImagesToDelete.Any())
                    {
                        foreach (var imageId in ImagesToDelete)
                        {
                            var image = db.Images.FirstOrDefault(i => i.ImageID == imageId && i.ProductID == product.ProductID);
                            if (image != null)
                            {
                                string imagePath = Path.Combine(Server.MapPath("~/Content/Images"), image.ImageName);
                                if (System.IO.File.Exists(imagePath))
                                {
                                    System.IO.File.Delete(imagePath);
                                }
                                db.Images.Remove(image);
                            }
                        }
                        db.SaveChanges();
                    }

                    // Thêm ảnh mới
                    if (ImageFiles != null && ImageFiles.Any(f => f != null && f.ContentLength > 0))
                    {
                        string thuMuc = Server.MapPath("~/Content/Images/");
                        if (!Directory.Exists(thuMuc))
                        {
                            Directory.CreateDirectory(thuMuc);
                        }

                        foreach (var file in ImageFiles.Where(f => f != null && f.ContentLength > 0))
                        {
                            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                            string extension = Path.GetExtension(file.FileName).ToLower();
                            if (!allowedExtensions.Contains(extension))
                            {
                                ModelState.AddModelError("ImageFiles", "Chỉ chấp nhận tệp .jpg, .jpeg hoặc .png!");
                                continue;
                            }
                            if (file.ContentLength > 5 * 1024 * 1024) // 5MB
                            {
                                ModelState.AddModelError("ImageFiles", "Tệp ảnh không được vượt quá 5MB!");
                                continue;
                            }

                            string tenFileGoc = Path.GetFileNameWithoutExtension(file.FileName);
                            string tenFile = $"{tenFileGoc}_{DateTime.Now.Ticks}{extension}";
                            string duongDanAnh = Path.Combine(thuMuc, tenFile);
                            file.SaveAs(duongDanAnh);

                            db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                        }
                        db.SaveChanges();
                    }

                    if (ModelState.IsValid)
                    {
                        TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                        Debug.WriteLine($"Product Updated: ID={product.ProductID}, Name={product.ProductName}");
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error updating product: {ex.Message}");
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật sản phẩm: " + ex.Message);
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.CurrentPage = "Products";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Vui lòng truyền ID!";
                return RedirectToAction("Index");
            }
            Product product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.ProductID == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                return RedirectToAction("Index");
            }
            Debug.WriteLine($"Product Delete View: ID={id}, Name={product.ProductName}");
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = db.Products
                    .Include(p => p.Images)
                    .FirstOrDefault(p => p.ProductID == id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                    return RedirectToAction("Index");
                }

                foreach (var image in product.Images.ToList())
                {
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Images"), image.ImageName);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    db.Images.Remove(image);
                }

                db.Products.Remove(product);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
                Debug.WriteLine($"Product Deleted: ID={id}");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa sản phẩm: " + ex.Message;
                Debug.WriteLine($"Error deleting product ID={id}: {ex.Message}");
            }
            return RedirectToAction("Index");
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