using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using System.Data.Entity;

namespace BTLWebMVC.Controllers
{
    public class HomeManagerController : Controller
    {
        private Context db = new Context();

        public ActionResult Index(int? month, int? year)
        {
            ViewBag.CurrentPage = "Dashboard";
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.donMoi = ThongKeDonHangTheoThang(selectedMonth, selectedYear);
            ViewBag.doanhThu = doanhThuThang(selectedMonth, selectedYear);
            ViewBag.ChartData = GetChartData(selectedMonth, selectedYear);
            return View();
        }

        [HttpPost]
        public JsonResult GetDashboardData(int month, int year)
        {
            var donMoi = ThongKeDonHangTheoThang(month, year);
            var doanhThu = doanhThuThang(month, year);
            var chartData = GetChartData(month, year);
            return Json(new
            {
                success = true,
                donMoi = donMoi,
                doanhThu = doanhThu,
                chartData = chartData
            });
        }

        public int ThongKeDonHangTheoThang(int month, int year)
        {
            int demDonHang = db.Orders
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
                .Count();
            return demDonHang;
        }

        public decimal doanhThuThang(int month, int year)
        {
            var ordersInMonth = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year)
                .ToList();

            if (!ordersInMonth.Any())
            {
                return 0;
            }

            decimal doanhthu = ordersInMonth
                .SelectMany(o => o.OrderDetails)
                .Sum(d => (decimal?)(d.Quantity * d.UnitPrice) ?? 0);

            return doanhthu;
        }

        public List<object> GetChartData(int month, int year)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var chartData = new List<object>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var dailyOrders = db.Orders
                    .Include(o => o.OrderDetails)
                    .Where(o => o.OrderDate.Month == month &&
                                o.OrderDate.Year == year &&
                                o.OrderDate.Day == day)
                    .ToList();

                decimal dailyRevenue = dailyOrders
                    .SelectMany(o => o.OrderDetails)
                    .Sum(d => (decimal?)(d.Quantity * d.UnitPrice) ?? 0);
                int orderCount = dailyOrders.Count;

                chartData.Add(new
                {
                    Day = day,
                    Revenue = dailyRevenue,
                    OrderCount = orderCount
                });
            }

            return chartData;
        }
    }
}