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
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace BTLWebMVC.Controllers.Manager
{
    public class OrdersController : Controller
    {
        // GET: Orders
        private Context db = new Context();
        // danh sách đơn hàng
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.OrderDetails).Include(o => o.Employee).Include(o => o.Customer);
            return View(orders.ToList());
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

        // hamf xuat hoa don -> dinh dang pdf
        public ActionResult PrintInvoice(int id)
        {
            var order = db.Orders.Include("OrderDetails.Product").FirstOrDefault(o => o.OrderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();


            // tieu de cho noi dung xuat
            var fontTieuDe = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);

            Paragraph tieuDe = new Paragraph("HÓA ĐƠN KHÁCH HÀNG", fontTieuDe);
            tieuDe.Alignment = Element.ALIGN_CENTER;
            document.Add(tieuDe);
            document.Add(new Paragraph("\n"));

            // thong tin don hang can xuat
            document.Add(new Paragraph($"Mã đơn hàng: {order.OrderID}"));
            document.Add(new Paragraph($"Khách hàng: {order.Customer.ContactName}"));
            document.Add(new Paragraph($"Nhân viên: {order.Employee.FirstName} {order.Employee.LastName}"));
            document.Add(new Paragraph($"Ngày đặt hàng: {order.OrderDate:dd/MM/yyyy}"));
            document.Add(new Paragraph($"Ngày giao hàng: {order.ShippedDate:dd/MM/yyyy}"));
            document.Add(new Paragraph($"Địa chỉ giao: {order.ShipAddress}"));
            document.Add(new Paragraph("\n"));
            // bang thong tin don hang
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 20, 40, 15, 15, 15 });

            table.AddCell("Mã SP");
            table.AddCell("Tên sản phẩm");
            table.AddCell("Số lượng");
            table.AddCell("Đơn giá");
            table.AddCell("Thành tiền");
            // duyet data cho bang nay
            foreach(var chitiet in order.OrderDetails)
            {
                table.AddCell(chitiet.Product.ProductID.ToString());
                table.AddCell(chitiet.Product.ProductName);
                table.AddCell(chitiet.Quantity.ToString());
                table.AddCell(chitiet.UnitPrice.ToString("N0") + " VND");
                table.AddCell((chitiet.Quantity * chitiet.UnitPrice).ToString("N0") + " VND");

            }
            document.Add(table);
            document.Close();
            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            return File(bytes, "application/pdf", $"HoaDon_{order.OrderID}.pdf");

        }

    }
}