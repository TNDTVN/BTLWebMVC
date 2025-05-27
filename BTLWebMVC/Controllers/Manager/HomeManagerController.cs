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
            ViewBag.OrderCount = ThongKeDonHangTheoThang(selectedMonth, selectedYear);
            ViewBag.Revenue = doanhThuThang(selectedMonth, selectedYear);
            ViewBag.NewCustomerCount = NewCustomerCount(selectedMonth, selectedYear);
            ViewBag.ProductCount = ProductCount(selectedMonth, selectedYear);
            ViewBag.ChartData = GetChartData(selectedMonth, selectedYear);
            return View();
        }

        [HttpPost]
        public JsonResult GetDashboardData(int month, int year)
        {
            var orderCount = ThongKeDonHangTheoThang(month, year);
            var revenue = doanhThuThang(month, year);
            var newCustomerCount = NewCustomerCount(month, year);
            var productCount = ProductCount(month, year);
            var chartData = GetChartData(month, year);
            return Json(new
            {
                success = true,
                orderCount = orderCount,
                revenue = revenue,
                newCustomerCount = newCustomerCount,
                productCount = productCount,
                chartData = chartData
            });
        }

        public int ThongKeDonHangTheoThang(int month, int year)
        {
            return db.Orders
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year
                            && o.EmployeeID != null && !o.IsCancelled) // Loại bỏ hóa đơn chưa duyệt và đã hủy
                .Count();
        }

        public decimal doanhThuThang(int month, int year)
        {
            var ordersInMonth = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year
                            && o.EmployeeID != null && !o.IsCancelled) // Loại bỏ hóa đơn chưa duyệt và đã hủy
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

        public int NewCustomerCount(int month, int year)
        {
            var firstOrderDates = db.Orders
                .Where(o => o.EmployeeID != null && !o.IsCancelled) // Loại bỏ hóa đơn chưa duyệt và đã hủy
                .GroupBy(o => o.CustomerID)
                .Select(g => new { CustomerID = g.Key, FirstOrderDate = g.Min(o => o.OrderDate) })
                .Where(o => o.FirstOrderDate.Month == month && o.FirstOrderDate.Year == year);

            return firstOrderDates.Count();
        }

        public int ProductCount(int month, int year)
        {
            var ordersInMonth = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year
                            && o.EmployeeID != null && !o.IsCancelled) // Loại bỏ hóa đơn chưa duyệt và đã hủy
                .ToList();

            if (!ordersInMonth.Any())
            {
                return 0;
            }

            return ordersInMonth
                .SelectMany(o => o.OrderDetails)
                .Sum(d => d.Quantity);
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
                                o.OrderDate.Day == day &&
                                o.EmployeeID != null && !o.IsCancelled) 
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