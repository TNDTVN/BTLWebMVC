using BTLWebMVC.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.Models;
using System.Data.Entity;

namespace BTLWebMVC.Controllers
{
    public class HomeManagerController : Controller
    {
        private Context db = new Context();
        public ActionResult Index()
        {
            ViewBag.donMoi = ThongKeDonHangTheoThang();
            ViewBag.doanhThu = doanhThuThang();
            return View();
        }
        // thong ke số lượng don hang trong thang
        public int ThongKeDonHangTheoThang()
        {
            int thangHienTai = DateTime.Now.Month;
            int namHienTai = DateTime.Now.Year;

            int demDonHang = db.Orders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate).Value.Month == thangHienTai
                         && DbFunctions.TruncateTime(o.OrderDate).Value.Year == namHienTai)
                .Count();

            return demDonHang;
        }
        public decimal doanhThuThang()
        {
            int thangHienTai = DateTime.Now.Month;
            int namHienTai = DateTime.Now.Year;

     

            var ordersInMonth = db.Orders
                .Where(o => DbFunctions.TruncateTime(o.OrderDate).Value.Month == thangHienTai &&
                            DbFunctions.TruncateTime(o.OrderDate).Value.Year == namHienTai)
                .ToList();

            if (!ordersInMonth.Any())
            {
               
                return 0;
            }

            decimal doanhthu = ordersInMonth
                .SelectMany(o => o.OrderDetails)
                .Sum(d => (decimal?)d.Quantity * d.UnitPrice ?? 0);


            return doanhthu;
        }

    }
}