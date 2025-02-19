using System;
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
            var products = db.Products.Include(p => p.Images)
                                      .Include(p => p.Category)
                                      .OrderByDescending(x => x.ProductID)
                                      .Take(20) 
                                      .ToList();

            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            return View(products); 
        }

        public ActionResult Categories(int? page, string sortOrder, string searchString, decimal? minPrice, decimal? maxPrice, int? categoryId)
        {
            int pageSizeValue = 12;
            int pageNumber = page ?? 1;

            ViewBag.Categories = db.Categories.AsNoTracking().ToList();

            // Lấy giá trị min và max cho giá sản phẩm
            var priceRange = db.Products.Select(p => p.UnitPrice);
            ViewBag.PriceMin = minPrice ?? priceRange.Min();
            ViewBag.PriceMax = maxPrice ?? priceRange.Max();

            var products = db.Products.AsQueryable();

            // Tìm kiếm sản phẩm theo tên
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString));
            }

            // Lọc theo giá
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.UnitPrice <= maxPrice.Value);
            }

            // Lọc theo danh mục (category)
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
            // Phân trang và nạp dữ liệu liên quan
            var pagedProducts = products
                                .Include(p => p.Images)
                                .Include(p => p.Category)
                                .AsNoTracking()
                                .ToPagedList(pageNumber, pageSizeValue);

            return View(pagedProducts);
        }

        public ActionResult Details(int id) 
        { 
            return View();
        }
    }
}