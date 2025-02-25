using BTLWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace BTLWebMVC.App_Start
{
    public class DbInitializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            var accounts = new List<Account>()
                    {
                        new Account {Username = "admin",
                        Password = "admin123",
                        Email = "admin@vtnn.com",
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Admin",
                        TokenCode = null
                        },
                        new Account {Username = "customer",
                        Password = "customer123",
                        Email = "customer@vtnn.com",
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Customer",
                        TokenCode= null
                        },
                        new Account {Username = "employee",
                        Password = "employee123",
                        Email = "employee@vtnn.com",
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Employee",
                        TokenCode = null
                        }

                    };
            context.Accounts.AddRange(accounts);

            var categories = new List<Category>
                    {
                        new Category { CategoryName = "Linh kiện", Description = "Linh kiện máy tính" },
                        new Category { CategoryName = "Phụ kiện", Description = "Phụ kiện công nghệ" },
                        new Category { CategoryName = "Thiết bị mạng", Description = "Thiết bị mạng" },
                        new Category { CategoryName = "Ngoại vi", Description = "Thiết bị ngoại vi" }
                    };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var suppliers = new List<Supplier>
                    {
                        new Supplier
                        {
                            SupplierName = "Cong Ty Phan Bon ABC",
                            ContactName = "Nguyen H",
                            Address = "101 Phan Boi Chau",
                            City = "Hanoi",
                            PostalCode = "10000",
                            Country = "Vietnam",
                            Phone = "0123456789",
                            Email = "abc@suppliers.com"
                        },
                        new Supplier
                        {
                            SupplierName = "Cong Ty Vat Tu XYZ",
                            ContactName = "Le K",
                            Address = "202 Le Thanh Ton",
                            City = "HCM City",
                            PostalCode = "70000",
                            Country = "Vietnam",
                            Phone = "0987654321",
                            Email = "xyz@suppliers.com"
                        }
                    };
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            var products = new List<Product>
                    {
                        new Product { ProductName = "Intel core I9-13900k", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 12900000m, UnitsInStock = 100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ xử lý - CPU Intel Core I9_13900k" },
                        new Product { ProductName = "AMD Radeon™ RX 7900 XTX 24G – 24GB GDDR6", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 31500000m, UnitsInStock = 150, UnitsOnOrder = 20, Discontinued = false, ProductDescription = "AMD Radeon™ RX 7900 XTX 24G – 24GB GDDR6" },
                        new Product { ProductName = "Mainboard Bo mạch chủ MSI A320M A PRO", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "5 kg", UnitPrice = 1270000m, UnitsInStock = 120, UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Mainboard Bo mạch chủ MSI A320M A PRO (AMD A320, Socket AM4, m-ATX, 2 khe RAM DDR4)" },
                        new Product { ProductName = "Bộ nguồn Corsair", CategoryID = 1, SupplierID = 2, QuantityPerUnit = "500 g", UnitPrice = 4350000m, UnitsInStock = 200, UnitsOnOrder = 30, Discontinued = false, ProductDescription = "Corsair RMx Series RM850x 850 Watt Power Supply, 80 PLUS Gold, PSU, Fully Modular, ATX, Black, UK, (2021) | CP-9020200-UK" },
                        new Product { ProductName = "Bộ nhớ trong DDR4", CategoryID = 1, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 2350000m, UnitsInStock = 250, UnitsOnOrder = 40, Discontinued = false, ProductDescription = "TeamGroup TF3D432G3600HC18JDC01 T-Force Delta RGB 32GB (2x16GB) DDR4-3600MHz CL18 1.35V Black Desktop Memory" },
                        new Product { ProductName = "Ổ cứng lưu trữ Toshiba 4TB", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "100 ml", UnitPrice = 12250000m, UnitsInStock = 300, UnitsOnOrder = 50, Discontinued = false, ProductDescription = "Toshiba X300 4TB Performance Desktop and Gaming Hard Drive 7200 RPM 128MB Cache SATA 6.0Gb/s 3.5 Inch Internal Hard Drive Retail Packaging HDWE140XZSTA" },
                        new Product { ProductName = "Quạt tản nhiệt NZXT", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "200 ml", UnitPrice = 3495000m, UnitsInStock = 147, UnitsOnOrder = 23, Discontinued = false, ProductDescription = "NZXT KRAKEN X63 RGB – 280MM AIO LIQUID COOLER WITH RGB FANS & INFINITY MIRROR DISPLAY" },
                        new Product { ProductName = "Bàn Phím Cơ", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 590000m, UnitsInStock = 60, UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Bàn Phím Cơ K3 Premium Gaming Red Switch Có Dây" },
                        new Product { ProductName = "Chuột gaming Logitech", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 416000m, UnitsInStock = 80, UnitsOnOrder = 20, Discontinued = false, ProductDescription = "Chuột gaming có dây Logitech G102 Gen 2 Lightsync RGB Black (910-005802)" },
                        new Product { ProductName = "Loa vi tính", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "25 kg", UnitPrice = 140000, UnitsInStock = 130, UnitsOnOrder = 25, Discontinued = false, ProductDescription = "Loa máy tinh SOUNDMAX A-130 (2.0) - Protable Speaker System ~ Loa di động dành cho Laptop, ĐTDĐ" },
                        new Product { ProductName = "Tai nghe gaming", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 249000m , UnitsInStock = 210, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tai nghe gaming Edra EH404 USB LED RGB" },
                        new Product { ProductName = "Micro", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 3299000m, UnitsInStock = 230, UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Microphone Kingston HyperX QuadCast S RGB - HMIQ1S-XX-RG/G" }
                        //new Product { ProductName = "Thuốc Diệt Cỏ", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "500 ml", UnitPrice = 45000m, UnitsInStock = 90, UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Thuốc diệt cỏ an toàn, không ảnh hưởng môi trường" },
                        //new Product { ProductName = "Cào Làm Vườn", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 40000m, UnitsInStock = 50, UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Cào làm vườn tiện dụng cho đất" },
                        //new Product { ProductName = "Phân NPK", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "25 kg", UnitPrice = 75000m, UnitsInStock = 157, UnitsOnOrder = 21, Discontinued = false, ProductDescription = "Phân bón tổng hợp NPK chất lượng cao" },
                        //new Product { ProductName = "Phân Lân", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "50 kg", UnitPrice = 65000m, UnitsInStock = 145, UnitsOnOrder = 30, Discontinued = false, ProductDescription = "Phân lân giúp cây phát triển rễ khỏe" },
                        //new Product { ProductName = "Hạt Giống Dưa Leo", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "50 g", UnitPrice = 13000m, UnitsInStock = 120, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Hạt giống dưa leo phát triển nhanh" },
                        //new Product { ProductName = "Hạt Giống Bắp Cải", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "50 g", UnitPrice = 14000m, UnitsInStock = 140, UnitsOnOrder = 9, Discontinued = false, ProductDescription = "Hạt giống bắp cải chất lượng cao" },
                        //new Product { ProductName = "Thuốc Trừ Bệnh Lá", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "250 ml", UnitPrice = 35000m, UnitsInStock = 85, UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Thuốc trừ bệnh lá hiệu quả" },
                        //new Product { ProductName = "Phân Bón Vi Sinh", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "10 kg", UnitPrice = 40000m, UnitsInStock = 110, UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Phân bón vi sinh cải tạo đất" },
                        //new Product { ProductName = "Hạt Giống Xà Lách", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 10000m, UnitsInStock = 250, UnitsOnOrder = 14, Discontinued = false, ProductDescription = "Hạt giống xà lách chất lượng" }
                    };
            context.Products.AddRange(products);
            context.SaveChanges();
            var images = new List<Image>();
            for (int i = 1; i <= 12; i++)
            {
                images.Add(new Image
                {
                    ProductID = i,
                    ImageName = $"anh{i}.jpg"
                });
            }
            context.Images.AddRange(images);
            context.SaveChanges();

            var customer = new Customer
            {
                CustomerName = "Ngô Tấn Phúc",
                ContactName = "Phúc",
                Address = "456 Đường Nhỏ, Quận 2",
                City = "TP. Hồ Chí Minh",
                PostalCode = "70001",
                Country = "Vietnam",
                Phone = "0909765432",
                Email = "customer@vtnn.com",
                Account = accounts[1]
            };
            context.Customers.Add(customer);

            var employee = new Employee
            {
                FirstName = "Lê",
                LastName = "Hồng C",
                BirthDate = new DateTime(1990, 1, 15),
                HireDate = DateTime.Now,
                Address = "789 Đường Trung, Quận 3",
                City = "TP. Hồ Chí Minh",
                PostalCode = "70002",
                Country = "Vietnam",
                Phone = "0912345678",
                Email = "admin@vtnn.com",
                Account = accounts[0]
            };
            context.Employees.Add(employee);

            var employee1 = new Employee
            {
                FirstName = "Đào",
                LastName = "Thanh Hào",
                BirthDate = new DateTime(2004, 10, 14),
                HireDate = DateTime.Now,
                Address = "Phú Thành A, Tam Nông",
                City = "Đồng Tháp",
                PostalCode = "70002",
                Country = "Việt Nam",
                Phone = "0912345678",
                Email = "employee@vtnn.com",
                Account = accounts[2]
            };
            context.Employees.Add(employee1);

            var order = new Order
            {
                Customer = customer,
                Employee = employee,
                OrderDate = DateTime.Now,
                ShippedDate = DateTime.Now.AddDays(3),
                ShipAddress = "456 Đường Nhỏ, Quận 2",
                ShipCity = "TP. Hồ Chí Minh",
                ShipPostalCode = "70001",
                ShipCountry = "Vietnam",
                Notes = "Chuyển Nhanh",
                Freight = 20000m
            };
            context.Orders.Add(order);

            var orderDetail = new OrderDetail
            {
                Order = order,
                Product = products[0],
                UnitPrice = products[0].UnitPrice,
                Quantity = 10,
                Discount = 0.1f
            };
            context.OrderDetails.Add(orderDetail);

            context.SaveChanges();
        }
    }
}
