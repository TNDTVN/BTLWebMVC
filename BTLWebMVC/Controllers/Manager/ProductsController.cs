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
using System.IO;
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
                TempData["ErrorMessage"] = "Không có id sản phẩm!";
                return RedirectToAction("Index");
            }
            Product product = db.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefault(p => p.ProductID == id);
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
        public ActionResult Create([Bind(Include = "ProductID,ProductName,CategoryID,SupplierID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductDescription")] Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Products.Add(product);
                    db.SaveChanges();

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string thuMuc = Server.MapPath("~/Content/Images/");
                        if (!Directory.Exists(thuMuc))
                        {
                            Directory.CreateDirectory(thuMuc);
                        }
                        string tenFileGoc = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        string duoiFile = Path.GetExtension(ImageFile.FileName);
                        string tenFile = $"{tenFileGoc}_{DateTime.Now.Ticks}{duoiFile}";
                        string duongDanAnh = Path.Combine(thuMuc, tenFile);
                        ImageFile.SaveAs(duongDanAnh);

                        db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                        db.SaveChanges();
                    }
                    TempData["SuccessMessage"] = "Tạo sản phẩm thành công!";
                    Debug.WriteLine($"Product Created: ID={product.ProductID}, Name={product.ProductName}");
                    return RedirectToAction("Index");
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            Debug.WriteLine($"Product Edit: ID={id}, Name={product.ProductName}");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,CategoryID,SupplierID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductDescription")] Product product, HttpPostedFileBase FileAnh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    if (FileAnh != null && FileAnh.ContentLength > 0)
                    {
                        string tenFile = Path.GetFileName(FileAnh.FileName);
                        string duongDanAnh = Path.Combine(Server.MapPath("~/Content/Images"), tenFile);
                        FileAnh.SaveAs(duongDanAnh);

                        var anhHienTai = db.Images.FirstOrDefault(i => i.ProductID == product.ProductID);
                        if (anhHienTai != null)
                        {
                            anhHienTai.ImageName = tenFile;
                        }
                        else
                        {
                            db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                        }
                        db.SaveChanges();
                    }
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    Debug.WriteLine($"Product Updated: ID={product.ProductID}, Name={product.ProductName}");
                    return RedirectToAction("Index");
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
                TempData["ErrorMessage"] = "Vui lòng truyền id!";
                return RedirectToAction("Index");
            }
            Product product = db.Products.Find(id);
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
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy sản phẩm!";
                    return RedirectToAction("Index");
                }
                var images = db.Images.Where(i => i.ProductID == id).ToList();
                foreach (var image in images)
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