using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using System.Data.Entity;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Drawing.Chart;
using BTLWebMVC.Models;
using System.Diagnostics;

namespace BTLWebMVC.Controllers.Manager
{
    public class StatisticsController : Controller
    {
        private Context db = new Context();

        // GET: Statistics
        public ActionResult Index(string filterType = "Month", int? month = null, int? year = null)
        {
            ViewBag.CurrentPage = "Statistics";
            int selectedMonth = month ?? DateTime.Now.Month;
            int selectedYear = year ?? DateTime.Now.Year;
            ViewBag.SelectedMonth = selectedMonth;
            ViewBag.SelectedYear = selectedYear;
            ViewBag.FilterType = filterType;

            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing.");
                return RedirectToAction("Index", "Home");
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                Debug.WriteLine($"Account with ID {parsedAccountId} not found.");
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách đơn hàng
            var ordersQuery = db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Include(o => o.Customer)
                .Include(o => o.Employee);

            if (filterType == "Month")
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Month == selectedMonth && o.OrderDate.Year == selectedYear);
            }
            else
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Year == selectedYear);
            }

            if (account.Role != "Admin")
            {
                var employee = db.Employees.FirstOrDefault(e => e.AccountID == parsedAccountId);
                if (employee != null)
                {
                    ordersQuery = ordersQuery.Where(o => o.EmployeeID == employee.EmployeeID);
                }
                else
                {
                    Debug.WriteLine($"Employee with AccountID {parsedAccountId} not found.");
                    ordersQuery = ordersQuery.Where(o => false);
                }
            }

            var orders = ordersQuery.OrderBy(o => o.OrderDate).ToList();
            Debug.WriteLine($"Orders Count: {orders.Count}");

            // Tính dữ liệu cho card
            ViewBag.OrderCount = orders.Count;
            ViewBag.Revenue = orders.Any() ? orders.SelectMany(o => o.OrderDetails).Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0) : 0;
            ViewBag.CustomerCount = orders.Select(o => o.CustomerID).Distinct().Count();
            ViewBag.ProductCount = orders.Any() ? orders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity) : 0;
            Debug.WriteLine($"Card Data - OrderCount: {ViewBag.OrderCount}, Revenue: {ViewBag.Revenue}, CustomerCount: {ViewBag.CustomerCount}, ProductCount: {ViewBag.ProductCount}");

            // Dữ liệu biểu đồ
            ViewBag.ChartData = GetChartData(filterType, selectedMonth, selectedYear, account);
            Debug.WriteLine($"ChartData Count: {ViewBag.ChartData.Count}");

            return View(orders);
        }

        // POST: GetChartData
        [HttpPost]
        public JsonResult GetChartData(string filterType, int month, int year)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing in GetChartData.");
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                Debug.WriteLine($"Account with ID {parsedAccountId} not found in GetChartData.");
                return Json(new { success = false, message = "Tài khoản không tồn tại!" });
            }

            var chartData = GetChartData(filterType, month, year, account);
            Debug.WriteLine($"GetChartData Returned: {chartData.Count} items");
            return Json(new { success = true, chartData = chartData });
        }

        private List<object> GetChartData(string filterType, int month, int year, Account account)
        {
            var chartData = new List<object>();
            var ordersQuery = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => filterType == "Month" ? o.OrderDate.Month == month && o.OrderDate.Year == year : o.OrderDate.Year == year);

            if (account.Role != "Admin")
            {
                var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
                if (employee != null)
                {
                    ordersQuery = ordersQuery.Where(o => o.EmployeeID == employee.EmployeeID);
                }
                else
                {
                    Debug.WriteLine($"Employee with AccountID {account.AccountID} not found in GetChartData.");
                    ordersQuery = ordersQuery.Where(o => false);
                }
            }

            var orders = ordersQuery.ToList();
            Debug.WriteLine($"GetChartData Orders Count: {orders.Count}");

            if (filterType == "Month")
            {
                var daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var dailyOrders = orders
                        .Where(o => o.OrderDate.Day == day)
                        .ToList();

                    decimal dailyRevenue = dailyOrders
                        .SelectMany(o => o.OrderDetails)
                        .Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0);
                    int orderCount = dailyOrders.Count;

                    chartData.Add(new
                    {
                        period = day,
                        revenue = dailyRevenue,
                        orderCount = dailyOrders.Count
                    });
                }
            }
            else
            {
                for (int m = 1; m <= 12; m++)
                {
                    var monthlyOrders = orders
                        .Where(o => o.OrderDate.Month == m)
                        .ToList();

                    decimal monthlyRevenue = monthlyOrders
                        .SelectMany(o => o.OrderDetails)
                        .Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0);
                    int orderCount = monthlyOrders.Count;

                    chartData.Add(new
                    {
                        period = m,
                        revenue = monthlyRevenue,
                        orderCount = monthlyOrders.Count
                    });
                }
            }

            Debug.WriteLine($"GetChartData Returned: {chartData.Count} items, Non-zero Revenue: {chartData.Count(d => ((dynamic)d).revenue > 0)}, Non-zero Orders: {chartData.Count(d => ((dynamic)d).orderCount > 0)}");
            return chartData;
        }

        // POST: Statistics/ExportToExcel
        [HttpPost]
        public ActionResult ExportToExcel(string filterType, int month, int year)
        {
            var accountId = Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId) || !int.TryParse(accountId, out int parsedAccountId))
            {
                Debug.WriteLine("Session AccountId invalid or missing in ExportToExcel.");
                return new HttpStatusCodeResult(401);
            }

            var account = db.Accounts.FirstOrDefault(a => a.AccountID == parsedAccountId);
            if (account == null)
            {
                Debug.WriteLine($"Account with ID {parsedAccountId} not found in ExportToExcel.");
                return new HttpStatusCodeResult(401);
            }

            var ordersQuery = db.Orders
                .Include(o => o.OrderDetails.Select(od => od.Product))
                .Include(o => o.Customer)
                .Include(o => o.Employee);

            if (filterType == "Month")
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Month == month && o.OrderDate.Year == year);
            }
            else
            {
                ordersQuery = ordersQuery.Where(o => o.OrderDate.Year == year);
            }

            if (account.Role != "Admin")
            {
                var employee = db.Employees.FirstOrDefault(e => e.AccountID == parsedAccountId);
                if (employee != null)
                {
                    ordersQuery = ordersQuery.Where(o => o.EmployeeID == employee.EmployeeID);
                }
                else
                {
                    Debug.WriteLine($"Employee with AccountID {parsedAccountId} not found in ExportToExcel.");
                    ordersQuery = ordersQuery.Where(o => false);
                }
            }

            var orders = ordersQuery.OrderBy(o => o.OrderDate).ToList();
            Debug.WriteLine($"ExportToExcel Orders Count: {orders.Count}");

            // Tính dữ liệu cho card
            var orderCount = orders.Count;
            var revenue = orders.Any() ? orders.SelectMany(o => o.OrderDetails).Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0) : 0;
            var customerCount = orders.Select(o => o.CustomerID).Distinct().Count();
            var productCount = orders.Any() ? orders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity) : 0;
            Debug.WriteLine($"Excel Card Data - OrderCount: {orderCount}, Revenue: {revenue}, CustomerCount: {customerCount}, ProductCount: {productCount}");

            using (var package = new ExcelPackage())
            {
                // Sheet: Thống kê đơn hàng
                var worksheet = package.Workbook.Worksheets.Add("Thống kê đơn hàng");

                // Tiêu đề
                worksheet.Cells[1, 1].Value = filterType == "Month" ? $"Thống kê đơn hàng tháng {month}/{year}" : $"Thống kê đơn hàng năm {year}";
                worksheet.Cells[1, 1, 1, 6].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Style.Font.Size = 14;
                worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Thống kê card
                worksheet.Cells[3, 1].Value = "Số đơn hàng";
                worksheet.Cells[3, 2].Value = orderCount;
                worksheet.Cells[4, 1].Value = "Doanh thu";
                worksheet.Cells[4, 2].Value = revenue;
                worksheet.Cells[4, 2].Style.Numberformat.Format = "#,##0";
                worksheet.Cells[5, 1].Value = "Số khách hàng";
                worksheet.Cells[5, 2].Value = customerCount;
                worksheet.Cells[6, 1].Value = "Số sản phẩm bán ra";
                worksheet.Cells[6, 2].Value = productCount;
                worksheet.Cells[3, 1, 6, 1].Style.Font.Bold = true;

                // Tiêu đề bảng đơn hàng
                worksheet.Cells[8, 1].Value = "Mã đơn hàng";
                worksheet.Cells[8, 2].Value = "Khách hàng";
                worksheet.Cells[8, 3].Value = "Nhân viên";
                worksheet.Cells[8, 4].Value = "Ngày đặt";
                worksheet.Cells[8, 5].Value = "Tổng tiền";
                worksheet.Cells[8, 6].Value = "Ghi chú";
                worksheet.Cells[8, 1, 8, 6].Style.Font.Bold = true;
                worksheet.Cells[8, 1, 8, 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[8, 1, 8, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                // Dữ liệu bảng đơn hàng
                int startRow = 9;
                for (int i = 0; i < orders.Count; i++)
                {
                    var order = orders[i];
                    worksheet.Cells[i + startRow, 1].Value = order.OrderID;
                    worksheet.Cells[i + startRow, 2].Value = order.Customer?.ContactName ?? "N/A";
                    worksheet.Cells[i + startRow, 3].Value = order.Employee != null ? $"{order.Employee.FirstName} {order.Employee.LastName}" : "Chưa phân công";
                    worksheet.Cells[i + startRow, 4].Value = order.OrderDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + startRow, 5].Value = order.OrderDetails.Sum(od => od.Quantity * od.UnitPrice);
                    worksheet.Cells[i + startRow, 5].Style.Numberformat.Format = "#,##0";
                    worksheet.Cells[i + startRow, 6].Value = order.Notes ?? "Không có";
                }

                // Dữ liệu biểu đồ (dùng ChartData cho Excel)
                var chartData = GetChartDataForExcel(filterType, month, year, account);
                Debug.WriteLine($"ExportToExcel ChartData Count: {chartData.Count}");
                int chartDataStartRow = startRow + orders.Count + 2; // Dưới bảng đơn hàng, cách 2 hàng
                int hiddenDataStartRow = 100; // Ghi dữ liệu vào vùng ẩn (hàng 100)

                if (chartData.Any())
                {
                    // Ghi dữ liệu biểu đồ vào vùng ẩn
                    worksheet.Cells[hiddenDataStartRow, 1].Value = filterType == "Month" ? "Ngày" : "Tháng";
                    worksheet.Cells[hiddenDataStartRow, 2].Value = "Doanh thu (VNĐ)";
                    worksheet.Cells[hiddenDataStartRow, 3].Value = "Số đơn hàng";
                    for (int i = 0; i < chartData.Count; i++)
                    {
                        var data = chartData[i];
                        worksheet.Cells[i + hiddenDataStartRow + 1, 1].Value = data.Period;
                        worksheet.Cells[i + hiddenDataStartRow + 1, 2].Value = data.Revenue;
                        worksheet.Cells[i + hiddenDataStartRow + 1, 2].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[i + hiddenDataStartRow + 1, 3].Value = data.OrderCount;
                    }

                    // Tạo biểu đồ cột cho doanh thu
                    var chart = worksheet.Drawings.AddChart("ThongKeChart", eChartType.ColumnClustered);
                    chart.Title.Text = filterType == "Month" ? $"Doanh thu và số đơn hàng tháng {month}/{year}" : $"Doanh thu và số đơn hàng năm {year}";
                    chart.SetPosition(chartDataStartRow, 0, 0, 0); // Dưới bảng đơn hàng
                    chart.SetSize(800, 400);

                    // Series cho doanh thu
                    var revenueSeries = chart.Series.Add(worksheet.Cells[hiddenDataStartRow + 1, 2, hiddenDataStartRow + chartData.Count, 2],
                                                        worksheet.Cells[hiddenDataStartRow + 1, 1, hiddenDataStartRow + chartData.Count, 1]);
                    revenueSeries.Header = "Doanh thu (VNĐ)";
                    revenueSeries.Fill.Color = System.Drawing.Color.SkyBlue;

                    // Tạo biểu đồ đường cho số đơn hàng (trên trục Y phụ)
                    var lineChart = chart.PlotArea.ChartTypes.Add(eChartType.Line);
                    var orderSeries = lineChart.Series.Add(worksheet.Cells[hiddenDataStartRow + 1, 3, hiddenDataStartRow + chartData.Count, 3],
                                                          worksheet.Cells[hiddenDataStartRow + 1, 1, hiddenDataStartRow + chartData.Count, 1]);
                    orderSeries.Header = "Số đơn hàng";
                    orderSeries.Fill.Color = System.Drawing.Color.OrangeRed;

                    // Cấu hình trục Y phụ cho số đơn hàng
                    lineChart.UseSecondaryAxis = true;
                    lineChart.YAxis.Title.Text = "Số đơn hàng";
                    chart.YAxis.Title.Text = "Doanh thu (VNĐ)";
                    chart.XAxis.Title.Text = filterType == "Month" ? "Ngày" : "Tháng";
                    chart.Legend.Position = eLegendPosition.Bottom;
                }
                else
                {
                    worksheet.Cells[chartDataStartRow, 1].Value = "Không có dữ liệu để hiển thị biểu đồ.";
                    worksheet.Cells[chartDataStartRow, 1, chartDataStartRow, 6].Merge = true;
                    worksheet.Cells[chartDataStartRow, 1, chartDataStartRow, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Tự động điều chỉnh cột
                worksheet.Cells[startRow, 1, startRow + orders.Count, 6].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = filterType == "Month" ? $"ThongKeDonHang_Thang{month}_{year}.xlsx" : $"ThongKeDonHang_Nam{year}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        private List<ChartData> GetChartDataForExcel(string filterType, int month, int year, Account account)
        {
            var chartData = new List<ChartData>();
            var ordersQuery = db.Orders
                .Include(o => o.OrderDetails)
                .Where(o => filterType == "Month" ? o.OrderDate.Month == month && o.OrderDate.Year == year : o.OrderDate.Year == year);

            if (account.Role != "Admin")
            {
                var employee = db.Employees.FirstOrDefault(e => e.AccountID == account.AccountID);
                if (employee != null)
                {
                    ordersQuery = ordersQuery.Where(o => o.EmployeeID == employee.EmployeeID);
                }
                else
                {
                    Debug.WriteLine($"Employee with AccountID {account.AccountID} not found in GetChartDataForExcel.");
                    ordersQuery = ordersQuery.Where(o => false);
                }
            }

            var orders = ordersQuery.ToList();
            Debug.WriteLine($"GetChartDataForExcel Orders Count: {orders.Count}");

            if (filterType == "Month")
            {
                var daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    var dailyOrders = orders
                        .Where(o => o.OrderDate.Day == day)
                        .ToList();

                    decimal dailyRevenue = dailyOrders
                        .SelectMany(o => o.OrderDetails)
                        .Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0);
                    int orderCount = dailyOrders.Count;

                    chartData.Add(new ChartData
                    {
                        Period = day,
                        Revenue = dailyRevenue,
                        OrderCount = orderCount
                    });
                }
            }
            else
            {
                for (int m = 1; m <= 12; m++)
                {
                    var monthlyOrders = orders
                        .Where(o => o.OrderDate.Month == m)
                        .ToList();

                    decimal monthlyRevenue = monthlyOrders
                        .SelectMany(o => o.OrderDetails)
                        .Sum(od => (decimal?)(od.Quantity * od.UnitPrice) ?? 0);
                    int orderCount = monthlyOrders.Count;

                    chartData.Add(new ChartData
                    {
                        Period = m,
                        Revenue = monthlyRevenue,
                        OrderCount = orderCount
                    });
                }
            }

            Debug.WriteLine($"GetChartDataForExcel Returned: {chartData.Count} items, Non-zero Revenue: {chartData.Count(d => d.Revenue > 0)}, Non-zero Orders: {chartData.Count(d => d.OrderCount > 0)}");
            return chartData;
        }
    }
}