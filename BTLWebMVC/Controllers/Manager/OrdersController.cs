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


using PagedList;

namespace BTLWebMVC.Controllers.Manager
{
    public class OrdersController : Controller
    {
        // GET: Orders
        private Context db = new Context();
        // danh sách đơn hàng
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
              
                    return RedirectToAction("Login", "Login");
                }
            }
            else
            {
            // -> chuyen ve trang dnag nhap
                return RedirectToAction("Login", "Login");
            }

            // Sắp xếp và phân trang
            var orders = ordersQuery
                .OrderBy(o => o.OrderID)
                .ToPagedList(pageNumber, pageSize);

            return View(orders);


        }

        // sua don hang
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null || order.EmployeeID != null)
            {
                return HttpNotFound();
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
                if (existingOrder == null || existingOrder.EmployeeID != null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các trường được phép chỉnh sửa
                existingOrder.ShipAddress = order.ShipAddress;
                existingOrder.ShipCity = order.ShipCity;
                existingOrder.ShipPostalCode = order.ShipPostalCode;
                existingOrder.ShipCountry = order.ShipCountry;
                existingOrder.ShippedDate = order.ShippedDate;
                existingOrder.Notes = order.Notes;

                db.Entry(existingOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }


        public ActionResult duyetDonHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = db.Orders.Find(id);
            if (order == null || order.EmployeeID != null)
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

            // lấy thông tin đơn hàng từ csdl
            var order = db.Orders.Include(o => o.OrderDetails.Select(od => od.Product)).Include(o => o.Customer).Include(o => o.Employee).FirstOrDefault(o => o.OrderID == id);


            // kiem tra don hang co ton tai 
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
                Document document = new Document(pdf);

                document.Add(new Paragraph("Địa chỉ: 783 Phạm Hữu Lầu, Phường 6, Cao Lãnh, Đồng Tháp")
                            .SetFont(regularFont)
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                document.Add(new Paragraph("SĐT: 0898543919 | Email: dhao3017@gmail.com")
                    .SetFont(regularFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));


                Paragraph separator = new Paragraph().SetBorderBottom(new SolidBorder(1));
                document.Add(separator);

                // tieu de cho hoa don\
                document.Add(new Paragraph("HÓA ĐƠN BÁN HÀNG")).SetFont(boldFont).SetFontSize(18).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);


        
                document.Add(separator);

                // thong tin don hang
                document.Add(new Paragraph($"Mã đơn hàng: {order.OrderID}").SetFont(regularFont));
                document.Add(new Paragraph($"Khách hàng: {order.Customer.ContactName}")
               .SetFont(regularFont));
                document.Add(new Paragraph($"Địa chỉ: {order.Customer.Address}, {order.Customer.City}")
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Ngày đặt hàng: {order.OrderDate:dd/MM/yyyy}")
                    .SetFont(regularFont));
                if (order.ShippedDate.HasValue)
                {
                    document.Add(new Paragraph($"Ngày giao hàng: {order.ShippedDate:dd/MM/yyyy}")
                        .SetFont(regularFont));
                }
                document.Add(new Paragraph($"Địa chỉ giao: {order.ShipAddress}")
                    .SetFont(regularFont));
                if (order.Employee != null)
                {
                    document.Add(new Paragraph($"Nhân viên: {order.Employee.FirstName} {order.Employee.LastName}")
                        .SetFont(regularFont));
                }

             
                document.Add(separator);


                // Tạo bảng chi tiết đơn hàng
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(5).UseAllAvailableWidth();
                table.AddHeaderCell(new Cell().Add(new Paragraph("Mã SP").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Tên sản phẩm").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Số lượng").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Đơn giá").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Thành tiền").SetFont(boldFont)));


                foreach (var detail in order.OrderDetails)
                {
                    int quantity = detail.Quantity;
                    decimal unitPrice = detail.UnitPrice;

                    table.AddCell(new Cell().Add(new Paragraph(detail.Product.ProductID.ToString()).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(detail.Product.ProductName).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(quantity.ToString()).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph($"{unitPrice:#,0 ₫}").SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph($"{(quantity * unitPrice):#,0 ₫}").SetFont(regularFont)));
                }

                document.Add(table);

                
                decimal total = order.OrderDetails.Sum(d => (d.Quantity) * (d.UnitPrice));
                document.Add(new Paragraph($"Tổng cộng: {total:#,0 ₫}")
                    .SetFont(boldFont)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                //document.Add(new LineSeparator(new SolidLine()));


                if (!string.IsNullOrEmpty(order.Notes))
                {
                    document.Add(new Paragraph($"Ghi chú: {order.Notes}")
                        .SetFont(regularFont));
                }


                document.Add(new Paragraph("Cảm ơn quý khách đã mua hàng!")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));


                document.Close();


                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", $"HoaDon_{order.OrderID}.pdf");
            }



    


        }






    }




}
