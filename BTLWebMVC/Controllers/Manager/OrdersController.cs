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
        // GET: Orders
        private Context db = new Context();

        public ActionResult Index(int? page)
        {


            int pageSize = 5;
            int pageNumber = (page ?? 1);


            var accountId = Session["AccountId"]?.ToString();
            Console.WriteLine("giá trị acocunt từ phien: ", accountId);
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
                    if (employee != null)
                    {
                        // Lọc đơn hàng:
                        // - Tất cả đơn hàng chưa duyệt (EmployeeID == null)
                        // - Các đơn hàng đã duyệt của nhân viên đăng nhập (EmployeeID == employee.EmployeeID)
                        ordersQuery = ordersQuery
                            .Where(o => o.EmployeeID == null || o.EmployeeID == employee.EmployeeID);
                    }
                    else
                    {
                        // ko tim thay don hang nhan vien thi tra ce danh sach rong
                        return View(new List<Order>().ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // -> chuyen ve trang dnag nhap
                return RedirectToAction("Index", "Home");
            }

            // Sắp xếp và phân trang
            var orders = ordersQuery
                .OrderBy(o => o.OrderID)
                .ToPagedList(pageNumber, pageSize);

            return View(orders);


        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                return RedirectToAction("Index", "Home");
            }
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
            if (employee == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }


            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "ContactName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", order.EmployeeID);

            // Debug: Kiểm tra dữ liệu
            if (order.OrderDate == null || order.ShippedDate == null)
            {
                Console.WriteLine("OrderDate or ShippedDate is null for OrderID: " + id);
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = db.Orders.Find(order.OrderID);
                if (existingOrder == null)
                {
                    return HttpNotFound();
                }

                existingOrder.ShipAddress = order.ShipAddress;
                existingOrder.ShipCity = order.ShipCity;
                existingOrder.ShipPostalCode = order.ShipPostalCode;
                existingOrder.ShipCountry = order.ShipCountry;
                existingOrder.ShippedDate = order.ShippedDate;
                existingOrder.Notes = order.Notes;
                existingOrder.CustomerID = order.CustomerID; // Cập nhật CustomerID từ dropdown
                existingOrder.EmployeeID = order.EmployeeID; // Cập nhật EmployeeID từ dropdown

                db.Entry(existingOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, trả lại form với dữ liệu đã nhập
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "ContactName", order.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "FirstName", order.EmployeeID);
            return View(order);
        }




        public ActionResult duyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            // lay thong tin account ti session
            var accountId = Session["AccountID"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                return RedirectToAction("Index", "Account");
            }
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                return HttpNotFound();
            }
            var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
            if (employee == null)
            {
                return HttpNotFound();
            }

            // Gán EmployeeID để đánh dấu đơn hàng đã duyệt
            order.EmployeeID = employee.EmployeeID;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Order order = db.Orders.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return HttpNotFound();
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

