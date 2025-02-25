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

namespace BTLWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        private Context db = new Context();


        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);
            return View(products.ToList());
        }

 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult Create()
        {
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
                        string thuMuc = Server.MapPath("~/Content/Images/"); // Lưu vào Content/Images
                        if (!Directory.Exists(thuMuc))
                        {
                            Directory.CreateDirectory(thuMuc);
                        }
                        string tenFileGoc = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        string duoiFile = Path.GetExtension(ImageFile.FileName);
                        string tenFile = $"{tenFileGoc}_{DateTime.Now.Ticks}{duoiFile}"; // Tạo tên file duy nhất
                        string duongDanAnh = Path.Combine(thuMuc, tenFile);
                        ImageFile.SaveAs(duongDanAnh);

                        db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu sản phẩm: " + ex.Message);
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,CategoryID,SupplierID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,Discontinued,ProductDescription")] Product product, HttpPostedFileBase FileAnh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                // upload anh
                if (FileAnh != null && FileAnh.ContentLength > 0)
                {
                    string tenFile = System.IO.Path.GetFileName(FileAnh.FileName);
                    string duongDanAnh = System.IO.Path.Combine(Server.MapPath("~/Content/Images"), tenFile);

                    FileAnh.SaveAs(duongDanAnh);

                    var AnhHienTai = db.Images.FirstOrDefault(i => i.ProductID == product.ProductID);
                    if(AnhHienTai != null)
                    {
                        AnhHienTai.ImageName = tenFile; // cap nhap anh moi
                    } 
                    else
                    {
                        db.Images.Add(new Image { ProductID = product.ProductID, ImageName = tenFile });
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
       
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "SupplierName", product.SupplierID);
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
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
