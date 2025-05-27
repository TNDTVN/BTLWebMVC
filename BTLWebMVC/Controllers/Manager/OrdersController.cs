using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;
using System.Data.Entity;
using System.Net;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.IO.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using PagedList;

namespace BTLWebMVC.Controllers.Manager
{
    public class OrdersController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(int? pendingPage, int? approvedPage, int? cancelledPage)
        {
            ViewBag.CurrentPage = "Orders";
            int pageSize = 5;

            var accountId = Session["AccountId"]?.ToString();
            var ordersQuery = db.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Employee)
                .Include(o => o.Customer);

            if (!string.IsNullOrEmpty(accountId) && int.TryParse(accountId, out int parsedAccountId))
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
                if (account != null)
                {
                    var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
                    if (account.Role != "Admin" && employee != null)
                    {
                        ordersQuery = ordersQuery
                            .Where(o => o.EmployeeID == null || o.EmployeeID == employee.EmployeeID);
                    }
                    else if (account.Role != "Admin")
                    {
                        TempData["ErrorMessage"] = "Tài khoản không có quyền truy cập đơn hàng.";
                        return View(new OrderViewModel());
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Phiên đăng nhập không hợp lệ.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem đơn hàng.";
                return RedirectToAction("Index", "Home");
            }

            // Tạo ViewModel
            var viewModel = new OrderViewModel
            {
                PendingOrders = ordersQuery
                    .Where(o => o.EmployeeID == null && !o.IsCancelled)
                    .OrderBy(o => o.OrderID)
                    .ToPagedList(pendingPage ?? 1, pageSize),

                ApprovedOrders = ordersQuery
                    .Where(o => o.EmployeeID != null && !o.IsCancelled)
                    .OrderBy(o => o.OrderID)
                    .ToPagedList(approvedPage ?? 1, pageSize),

                CancelledOrders = ordersQuery
                    .Where(o => o.IsCancelled)
                    .OrderBy(o => o.OrderID)
                    .ToPagedList(cancelledPage ?? 1, pageSize)
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult CancelOrderDetails(int? id, bool isCancelled = false)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Thiếu thông tin ID đơn hàng.";
                return RedirectToAction("Index");
            }

            var order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index");
            }

            ViewBag.IsCancelled = isCancelled;
            return PartialView("CancelOrderDetails", order);
        }

        [HttpPost]
        public ActionResult CancelOrder(int id, string reason)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index");
            }

            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Index");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Tài khoản không tồn tại.";
                return RedirectToAction("Index");
            }

            foreach (var detail in order.OrderDetails)
            {
                var product = db.Products.Find(detail.ProductID);
                if (product != null)
                {
                    product.UnitsInStock += detail.Quantity;
                    db.Entry(product).State = EntityState.Modified;
                }
            }
            order.IsCancelled = true;
            order.Notes = reason;
            order.EmployeeID = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID)?.EmployeeID;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return new JsonResult { Data = new { success = true } };
        }

        public ActionResult duyetDonHang(int? id)
        {
            ViewBag.CurrentPage = "Orders";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Thiếu thông tin ID đơn hàng.";
                return RedirectToAction("Index");
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index");
            }

            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                TempData["ErrorMessage"] = "Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Index");
            }
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Tài khoản không tồn tại.";
                return RedirectToAction("Index");
            }
            var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Nhân viên không tồn tại.";
                return RedirectToAction("Index");
            }

            order.EmployeeID = employee.EmployeeID;
            order.ShippedDate = DateTime.Now.AddDays(3);
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            ViewBag.CurrentPage = "Orders";
            if (id == null)
            {
                TempData["ErrorMessage"] = "Thiếu thông tin ID đơn hàng.";
                return RedirectToAction("Index");
            }
            Order order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index");
            }

            return View(order);
        }

        public ActionResult PrintInvoice(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails.Select(od => od.Product))
                                .Include(o => o.Customer)
                                .Include(o => o.Employee)
                                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng với ID: " + id;
                return RedirectToAction("Index");
            }

            string fontPath = Server.MapPath("~/Content/Fonts/arial-unicode-ms-regular.ttf");
            PdfFont regularFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            PdfFont boldFont = PdfFontFactory.CreateFont(Server.MapPath("~/Content/Fonts/arial-unicode-ms-bold.ttf"), PdfEncodings.IDENTITY_H);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.SetMargins(50, 50, 50, 50);

                DeviceRgb tealColor = new DeviceRgb(0, 128, 128);

                Paragraph title = new Paragraph("HÓA ĐƠN")
                    .SetFont(boldFont)
                    .SetFontSize(32)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(tealColor)
                    .SetMarginBottom(10);
                document.Add(title);

                Paragraph date = new Paragraph($"Ngày lập: {order.OrderDate:dd/MM/yyyy}")
                    .SetFont(regularFont)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMarginBottom(15);
                document.Add(date);

                Table infoTable = new Table(2).UseAllAvailableWidth()
                    .SetMarginBottom(20);
                infoTable.AddCell(new Cell().Add(new Paragraph("Hóa đơn cho:")
                    .SetFont(regularFont)
                    .SetFontSize(12))
                    .SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph("Thanh toán cho:")
                    .SetFont(regularFont)
                    .SetFontSize(12))
                    .SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"{order.Customer.ContactName}\n{order.Customer.Address}, {order.Customer.City}")
                    .SetFont(regularFont)
                    .SetFontSize(12))
                    .SetBorder(Border.NO_BORDER));
                infoTable.AddCell(new Cell().Add(new Paragraph($"CÔNG TY HPH FASHION\nSĐT: 03786679\nEmail: tphuong2000@gmail.com")
                    .SetFont(regularFont)
                    .SetFontSize(12))
                    .SetBorder(Border.NO_BORDER));
                document.Add(infoTable);

                LineSeparator separator = new LineSeparator(new SolidLine(1f))
                    .SetMarginTop(5)
                    .SetMarginBottom(15);
                document.Add(separator);

                Paragraph orderInfo = new Paragraph(
                    $"Mã đơn hàng: {order.OrderID}\n" +
                    (order.ShippedDate.HasValue ? $"Ngày giao hàng: {order.ShippedDate:dd/MM/yyyy}\n" : "") +
                    $"Địa chỉ giao: {order.ShipAddress}\n" +
                    (order.Employee != null ? $"Nhân viên: {order.Employee.FirstName} {order.Employee.LastName}\n" : ""))
                    .SetFont(regularFont)
                    .SetFontSize(12)
                    .SetMarginBottom(20);
                document.Add(orderInfo);

                document.Add(separator);

                Table table = new Table(new float[] { 1, 4, 1, 2, 2 }).UseAllAvailableWidth()
                    .SetMarginTop(10)
                    .SetMarginBottom(20);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Mã SP")
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(tealColor)
                    .SetFontColor(DeviceRgb.WHITE));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tên sản phẩm")
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(tealColor)
                    .SetFontColor(DeviceRgb.WHITE));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Số lượng")
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(tealColor)
                    .SetFontColor(DeviceRgb.WHITE));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá")
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(tealColor)
                    .SetFontColor(DeviceRgb.WHITE));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền")
                    .SetFont(boldFont)
                    .SetTextAlignment(TextAlignment.CENTER))
                    .SetBackgroundColor(tealColor)
                    .SetFontColor(DeviceRgb.WHITE));

                foreach (var detail in order.OrderDetails)
                {
                    int quantity = detail.Quantity;
                    decimal unitPrice = detail.UnitPrice;
                    decimal totalPrice = quantity * unitPrice;

                    table.AddCell(new Cell().Add(new Paragraph(detail.Product.ProductID.ToString())
                        .SetFont(regularFont)
                        .SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(detail.Product.ProductName)
                        .SetFont(regularFont)
                        .SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph(quantity.ToString())
                        .SetFont(regularFont)
                        .SetTextAlignment(TextAlignment.CENTER)));
                    table.AddCell(new Cell().Add(new Paragraph($"{unitPrice:#,0} ₫")
                        .SetFont(regularFont)
                        .SetTextAlignment(TextAlignment.RIGHT)));
                    table.AddCell(new Cell().Add(new Paragraph($"{totalPrice:#,0} ₫")
                        .SetFont(regularFont)
                        .SetTextAlignment(TextAlignment.RIGHT)));
                }

                document.Add(table);

                decimal total = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
                Paragraph totalParagraph = new Paragraph($"Tổng cộng: {total:#,0} ₫")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(tealColor)
                    .SetMarginTop(10);
                document.Add(totalParagraph);

                document.Add(separator);

                if (!string.IsNullOrEmpty(order.Notes))
                {
                    Paragraph notes = new Paragraph($"Ghi chú: {order.Notes}")
                        .SetFont(regularFont)
                        .SetFontSize(12)
                        .SetMarginTop(10)
                        .SetMarginBottom(20);
                    document.Add(notes);
                }

                Paragraph footer = new Paragraph()
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(20);
                footer.Add(new Text("⨀ CÔNG TY HPH Fashion\n").SetFont(boldFont).SetFontSize(14).SetFontColor(tealColor));
                footer.Add(new Text("CẢM ƠN QUÝ KHÁCH ĐÃ MUA HÀNG!\n")
                    .SetFont(regularFont)
                    .SetFontSize(12));
                footer.Add(new Text("CÔNG TY HPH FASHION\n")
                    .SetFont(regularFont)
                    .SetFontSize(12));
                footer.Add(new Text("783 Phạm Hữu Lầu, Phường 6, Cao Lãnh, Đồng Tháp\n")
                    .SetFont(regularFont)
                    .SetFontSize(12));
                footer.Add(new Text("SĐT: 0372698856, Email: tphuong2000@gmail.com")
                    .SetFont(regularFont)
                    .SetFontSize(12));
                document.Add(footer);

                document.Close();

                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", $"HoaDon_{order.OrderID}.pdf");
            }
        }
    }
}