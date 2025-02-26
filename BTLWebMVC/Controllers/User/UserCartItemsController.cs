using BTLWebMVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;

namespace BTLWebMVC.Controllers.User
{
    public class UserCartItemsController : Controller
    {
        Context db = new Context();
        // GET: UserCartItems
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HistoryProduct(int? page, int? accountid, string sortOrder, string searchString, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            int pageSizeValue = 12;
            int pageNumber = page ?? 1;

            ViewBag.Categories = db.Categories.AsNoTracking().ToList();

            var priceRange = db.Products.Select(p => p.UnitPrice);
            ViewBag.PriceMin = priceRange.Min();
            ViewBag.PriceMax = priceRange.Max();

            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountid);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng vui lòng đăng nhập!";
                return RedirectToAction("Index","Home");
            }

            var purchasedProductIds = db.OrderDetails
                                        .Where(od => od.Order.CustomerID == customer.CustomerID)
                                        .Select(od => od.ProductID)
                                        .Distinct();

            var products = db.Products.Where(p => purchasedProductIds.Contains(p.ProductID));

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
            ViewBag.accountId = accountid;
            var pagedProducts = products
                                .Include(p => p.Images)
                                .Include(p => p.Category)
                                .AsNoTracking()
                                .ToPagedList(pageNumber, pageSizeValue);

            return View(pagedProducts);
        }
    }
}