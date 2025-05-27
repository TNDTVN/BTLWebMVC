using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;
using System.Text.RegularExpressions;

namespace BTLWebMVC.Controllers.User
{
    public class UserCartItemsController : Controller
    {
        Context db = new Context();

        // GET: Hiển thị giỏ hàng
        public ActionResult Index()
        {
            if (Session["AccountId"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem giỏ hàng!";
                return RedirectToAction("Index", "Home");
            }

            int accountId = (int)Session["AccountId"];
            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng!";
                return RedirectToAction("Index", "Home");
            }

            var cartItems = db.CartItems
                .Where(c => c.CustomerID == customer.CustomerID)
                .Include(c => c.Product)
                .Include(c => c.Product.Images)
                .ToList();

            ViewBag.Customer = customer;
            return View(cartItems);
        }

        // POST: Xóa một sản phẩm khỏi giỏ hàng
        [HttpPost]
        public JsonResult RemoveItem(int cartItemId)
        {
            var cartItem = db.CartItems.Find(cartItemId);
            var product = db.Products.Find(cartItem?.ProductID);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }
            if (product == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại!" });
            }
            product.UnitsInStock += cartItem.Quantity;

            db.CartItems.Remove(cartItem);
            db.SaveChanges();

            return Json(new { success = true, message = "Đã xóa sản phẩm khỏi giỏ hàng!" });
        }

        // POST: Xóa tất cả sản phẩm trong giỏ hàng
        [HttpPost]
        public JsonResult ClearCart()
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            int accountId = (int)Session["AccountId"];
            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng!" });
            }

            var cartItems = db.CartItems.Where(c => c.CustomerID == customer.CustomerID).ToList();
            if (!cartItems.Any())
            {
                return Json(new { success = false, message = "Giỏ hàng đã trống!" });
            }
            foreach (var item in cartItems)
            {
                var product = db.Products.Find(item.ProductID);
                if (product != null)
                {
                    product.UnitsInStock += item.Quantity;
                }
            }
            db.CartItems.RemoveRange(cartItems);
            db.SaveChanges();

            return Json(new { success = true, message = "Đã xóa tất cả sản phẩm trong giỏ hàng!" });
        }

        // POST: Cập nhật số lượng sản phẩm
        [HttpPost]
        public JsonResult UpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1)
            {
                return Json(new { success = false, message = "Số lượng phải lớn hơn 0!" });
            }

            var cartItem = db.CartItems.Include(c => c.Product).FirstOrDefault(c => c.CartItemID == cartItemId);
            if (cartItem == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng!" });
            }

            int quantityDifference = quantity - cartItem.Quantity;

            if (quantityDifference != 0)
            {
                var product = cartItem.Product;
                if (product.UnitsInStock < quantityDifference)
                {
                    return Json(new { success = false, message = "Số lượng trong kho không đủ!" });
                }
                product.UnitsInStock -= quantityDifference;
            }

            cartItem.Quantity = quantity;
            db.SaveChanges();

            return Json(new { success = true, message = "Cập nhật số lượng thành công!" });
        }

        // POST: Chuyển giỏ hàng thành đơn hàng
        [HttpPost]
        public JsonResult CreateOrder(string customerName, string contactName, string phone, string email, string shipAddress, string shipCity, string shipPostalCode, string shipCountry, string notes)
        {
            if (Session["AccountId"] == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập!" });
            }

            int accountId = (int)Session["AccountId"];
            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId);
            if (customer == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin khách hàng!" });
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(customerName))
            {
                return Json(new { success = false, message = "Tên khách hàng không được để trống!" });
            }
            if (string.IsNullOrEmpty(contactName))
            {
                return Json(new { success = false, message = "Tên người liên hệ không được để trống!" });
            }
            if (string.IsNullOrEmpty(phone))
            {
                return Json(new { success = false, message = "Số điện thoại không được để trống!" });
            }
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email không được để trống!" });
            }
            if (string.IsNullOrEmpty(shipAddress))
            {
                return Json(new { success = false, message = "Địa chỉ giao hàng không được để trống!" });
            }
            if (string.IsNullOrEmpty(shipCity))
            {
                return Json(new { success = false, message = "Thành phố không được để trống!" });
            }
            if (string.IsNullOrEmpty(shipPostalCode))
            {
                return Json(new { success = false, message = "Mã bưu điện không được để trống!" });
            }
            if (string.IsNullOrEmpty(shipCountry))
            {
                return Json(new { success = false, message = "Quốc gia không được để trống!" });
            }

            // Kiểm tra độ dài tối đa (dựa trên model Customer)
            if (customerName.Length > 100)
            {
                return Json(new { success = false, message = "Tên khách hàng không được vượt quá 100 ký tự!" });
            }
            if (contactName.Length > 100)
            {
                return Json(new { success = false, message = "Tên người liên hệ không được vượt quá 100 ký tự!" });
            }
            if (phone.Length > 20)
            {
                return Json(new { success = false, message = "Số điện thoại không được vượt quá 20 ký tự!" });
            }
            if (email.Length > 100)
            {
                return Json(new { success = false, message = "Email không được vượt quá 100 ký tự!" });
            }
            if (shipAddress.Length > 100)
            {
                return Json(new { success = false, message = "Địa chỉ giao hàng không được vượt quá 100 ký tự!" });
            }
            if (shipCity.Length > 50)
            {
                return Json(new { success = false, message = "Thành phố không được vượt quá 50 ký tự!" });
            }
            if (shipPostalCode.Length > 10)
            {
                return Json(new { success = false, message = "Mã bưu điện không được vượt quá 10 ký tự!" });
            }
            if (shipCountry.Length > 50)
            {
                return Json(new { success = false, message = "Quốc gia không được vượt quá 50 ký tự!" });
            }
            if (!string.IsNullOrEmpty(notes) && notes.Length > 500) // Giả sử Notes tối đa 500 ký tự
            {
                return Json(new { success = false, message = "Ghi chú không được vượt quá 500 ký tự!" });
            }

            // Kiểm tra định dạng số điện thoại (Việt Nam)
            if (!Regex.IsMatch(phone, @"^(?:\+84|0)(3|5|7|8|9)\d{8}$"))
            {
                return Json(new { success = false, message = "Số điện thoại không đúng định dạng (VD: +849xxxxxxxx hoặc 09xxxxxxxx)." });
            }

            // Kiểm tra định dạng email
            if (!Regex.IsMatch(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return Json(new { success = false, message = "Email không đúng định dạng." });
            }

            // Kiểm tra định dạng tên khách hàng và tên liên hệ (chỉ chứa chữ cái, dấu cách, và ký tự tiếng Việt)
            if (!Regex.IsMatch(customerName, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                return Json(new { success = false, message = "Tên khách hàng chỉ được chứa chữ cái và dấu cách." });
            }
            if (!Regex.IsMatch(contactName, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                return Json(new { success = false, message = "Tên người liên hệ chỉ được chứa chữ cái và dấu cách." });
            }

            // Kiểm tra định dạng thành phố và quốc gia (chỉ chứa chữ cái, dấu cách, và ký tự tiếng Việt)
            if (!Regex.IsMatch(shipCity, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                return Json(new { success = false, message = "Thành phố chỉ được chứa chữ cái và dấu cách." });
            }
            if (!Regex.IsMatch(shipCountry, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                return Json(new { success = false, message = "Quốc gia chỉ được chứa chữ cái và dấu cách." });
            }

            // Kiểm tra định dạng mã bưu điện (chỉ chứa số)
            if (!Regex.IsMatch(shipPostalCode, @"^\d+$"))
            {
                return Json(new { success = false, message = "Mã bưu điện chỉ được chứa số." });
            }

            var cartItems = db.CartItems
                .Where(c => c.CustomerID == customer.CustomerID)
                .Include(c => c.Product)
                .ToList();

            if (!cartItems.Any())
            {
                return Json(new { success = false, message = "Giỏ hàng trống!" });
            }

            try
            {
                // Tạo đơn hàng mới
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = DateTime.Now,
                    EmployeeID = null,
                    ShippedDate = null,
                    ShipAddress = shipAddress,
                    ShipCity = shipCity,
                    ShipPostalCode = shipPostalCode,
                    ShipCountry = shipCountry,
                    Notes = notes,
                    Freight = 0,
                    IsCancelled = false
                };

                // Tạo chi tiết đơn hàng từ giỏ hàng
                order.OrderDetails = cartItems.Select(c => new OrderDetail
                {
                    ProductID = c.ProductID,
                    UnitPrice = c.UnitPrice,
                    Quantity = c.Quantity,
                    Discount = 0
                }).ToList();

                // Thêm đơn hàng và xóa giỏ hàng
                db.Orders.Add(order);
                db.CartItems.RemoveRange(cartItems);
                db.SaveChanges();

                return Json(new { success = true, message = "Đơn hàng đã được tạo thành công, đang chờ nhân viên duyệt!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra khi tạo đơn hàng: " + ex.Message });
            }
        }

        // Lịch sử mua hàng (giữ nguyên)
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
                return RedirectToAction("Index", "Home");
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

        public ActionResult PurchaseHistory(int? page)
        {
            if (Session["AccountId"] == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem lịch sử mua hàng!";
                return RedirectToAction("Index", "Home");
            }

            int accountId = (int)Session["AccountId"];
            var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng!";
                return RedirectToAction("Index", "Home");
            }

            int pageSize = 5;
            int pageNumber = page ?? 1;

            db.Configuration.ProxyCreationEnabled = false;

            var orders = db.Orders
                .Where(o => o.CustomerID == customer.CustomerID)
                .Include(o => o.OrderDetails)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Include(o => o.OrderDetails.Select(od => od.Product.Images))
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking();

            var pagedOrders = orders.ToPagedList(pageNumber, pageSize);

            db.Configuration.ProxyCreationEnabled = true;

            return View(pagedOrders);
        }
    }
}