﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BTLWebMVC.App_Start;
using System.Web.Helpers;
using System.Threading.Tasks;
using BTLWebMVC.Models;
using PagedList;

namespace BTLWebMVC.Controllers
{
    public class HomeController : Controller
    {

        private Context db = new Context();

        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            var categories = db.Categories.Take(4).ToList();

            foreach (var category in categories)
            {
                var categoryProducts = db.Products
                    .Include(p => p.Images)
                    .Include(p => p.Category)
                    .Where(p => p.CategoryID == category.CategoryID)
                    .OrderByDescending(x => x.ProductID)
                    .Take(5)
                    .ToList();

                products.AddRange(categoryProducts);
            }

            ViewBag.Categories = categories;
            ViewBag.SelectedCategory = null;

            return View(products);
        }

        [HttpGet]
        public ActionResult GetProducts(int? categoryId = null)
        {
            try
            {
                List<Product> products;

                if (categoryId == null) 
                {
                    products = new List<Product>();
                    var categories = db.Categories.Take(4).ToList();

                    foreach (var category in categories)
                    {
                        var categoryProducts = db.Products
                            .Include(p => p.Images)
                            .Include(p => p.Category)
                            .Where(p => p.CategoryID == category.CategoryID)
                            .OrderByDescending(x => x.ProductID)
                            .Take(5)
                            .ToList();

                        products.AddRange(categoryProducts);
                    }
                }
                else 
                {
                    products = db.Products
                        .Include(p => p.Images)
                        .Include(p => p.Category)
                        .Where(p => p.CategoryID == categoryId)
                        .OrderByDescending(x => x.ProductID)
                        .Take(20) 
                        .ToList();
                }

                return PartialView("GetProducts", products); 
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetProducts: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error: " + ex.Message);
            }
        }

        public ActionResult Categories(int? page, string sortOrder, string searchString, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            int pageSizeValue = 12;
            int pageNumber = page ?? 1;

            ViewBag.Categories = db.Categories.AsNoTracking().ToList();

            var priceRange = db.Products.Select(p => p.UnitPrice);
            ViewBag.PriceMin = priceRange.Min();
            ViewBag.PriceMax = priceRange.Max();

            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= maxPrice.Value);
            }

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryID == categoryId.Value);
            }

            switch (sortOrder)
            {
                case "price":
                    products = products.OrderBy(p => p.UnitPrice);
                    break;
                case "name":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductID);
                    break;
            }

            var pagedProducts = products
                                .Include(p => p.Images)
                                .Include(p => p.Category)
                                .AsNoTracking()
                                .ToPagedList(pageNumber, pageSizeValue);

            return View(pagedProducts);
        }
        [HttpGet]
        public ActionResult GetCategoryProducts(int? page, string sortOrder, string searchString, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            try
            {
                int pageSizeValue = 12;
                int pageNumber = page ?? 1;

                var products = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.ProductName.Contains(searchString));
                }

                if (minPrice.HasValue)
                {
                    products = products.Where(p => p.UnitPrice >= minPrice.Value);
                }
                if (maxPrice.HasValue)
                {
                    products = products.Where(p => p.UnitPrice <= maxPrice.Value);
                }

                if (categoryId.HasValue)
                {
                    products = products.Where(p => p.CategoryID == categoryId.Value);
                }

                switch (sortOrder)
                {
                    case "price":
                        products = products.OrderBy(p => p.UnitPrice);
                        break;
                    case "name":
                        products = products.OrderBy(p => p.ProductName);
                        break;
                    default:
                        products = products.OrderBy(p => p.ProductID);
                        break;
                }

                var pagedProducts = products
                                    .Include(p => p.Images)
                                    .Include(p => p.Category)
                                    .AsNoTracking()
                                    .ToPagedList(pageNumber, pageSizeValue);

                return PartialView("_CategoryProductList", pagedProducts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in GetCategoryProducts: " + ex.Message);
                return new HttpStatusCodeResult(500, "Internal Server Error: " + ex.Message);
            }
        }
        public ActionResult Details(int id) 
        {
            var products = db.Products.FirstOrDefault(p => p.ProductID == id);
            var images = db.Images.Where(p => p.ProductID == id).ToList();
            if (products == null)
            {
                TempData["ErrorMessage"] = "Không tồn tại sản phẩm với id: " + id;
                return RedirectToAction("Index","Home");
            }
            ViewBag.Images = images;
            ViewBag.ReturnUrl = Request.UrlReferrer?.ToString() ?? Url.Action("Index", "Home");
            return View(products);
        }
        [HttpPost]
        public JsonResult AddToCart(int productId, int quantity)
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm vào giỏ hàng!" });
            }

            var accountId = (int)Session["AccountId"];
            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng!" });
            }

            var product = db.Products.Include(p => p.Images).FirstOrDefault(p => p.ProductID == productId);
            if (product == null || product.UnitsInStock < quantity)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại hoặc số lượng không đủ!" });
            }

            var cartItem = db.CartItems.FirstOrDefault(ci => ci.CustomerID == customer.CustomerID && ci.ProductID == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                db.CartItems.Add(new CartItems
                {
                    CustomerID = customer.CustomerID,
                    ProductID = productId,
                    Quantity = quantity,
                    UnitPrice = product.UnitPrice,
                    ProductName = product.ProductName,
                    ImageUrl = product.Images.FirstOrDefault()?.ImageName
                });
            }

            product.UnitsInStock -= quantity; 
            db.SaveChanges();

            return Json(new { success = true });
        }

    }
}