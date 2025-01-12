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
                        ProfileImage = "profile.png",
                        CreatedDate = DateTime.Now,
                        Role = "Admin",
                        TokenCode = null
                        },
                        new Account {Username = "customer",
                        Password = "customer123",
                        Email = "customer@vtnn.com",
                        ProfileImage = "profile.png",
                        CreatedDate = DateTime.Now,
                        Role = "Customer",
                        TokenCode= null
                        },
                        new Account {Username = "employee",
                        Password = "employee123",
                        Email = "employee@vtnn.com",
                        ProfileImage = "profile.png",
                        CreatedDate = DateTime.Now,
                        Role = "Employee",
                        TokenCode = null
                        }

                    };
            context.Accounts.AddRange(accounts);

            var categories = new List<Category>
                    {
                        new Category { CategoryName = "Phân bón", Description = "Các loại phân cho cây trồng" },
                        new Category { CategoryName = "Hạt giống", Description = "Hạt giống các loại cây trồng nông nghiệp" },
                        new Category { CategoryName = "Thuốc trừ sâu", Description = "Các loại thuốc bảo vệ thực vật" },
                        new Category { CategoryName = "Công cụ", Description = "Các loại công cụ cho nông nghiệp" }
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
                        new Product { ProductName = "Phân Ure", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "25 kg", UnitPrice = 50000m, UnitsInStock = 100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Phân ure chất lượng cao cho cây trồng" },
                        new Product { ProductName = "Phân Kali", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "50 kg", UnitPrice = 65000m, UnitsInStock = 150, UnitsOnOrder = 20, Discontinued = false, ProductDescription = "Phân kali giúp cây phát triển bền vững" },
                        new Product { ProductName = "Phân DAP", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "50 kg", UnitPrice = 70000m, UnitsInStock = 120, UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Phân DAP nhập khẩu, hiệu quả cao" },
                        new Product { ProductName = "Hạt Giống Ngô", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "500 g", UnitPrice = 15000m, UnitsInStock = 200, UnitsOnOrder = 30, Discontinued = false, ProductDescription = "Hạt giống ngô năng suất cao" },
                        new Product { ProductName = "Hạt Giống Lúa", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 20000m, UnitsInStock = 250, UnitsOnOrder = 40, Discontinued = false, ProductDescription = "Hạt giống lúa bền vững, kháng sâu bệnh" },
                        new Product { ProductName = "Thuốc Trừ Sâu X", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "100 ml", UnitPrice = 25000m, UnitsInStock = 300, UnitsOnOrder = 50, Discontinued = false, ProductDescription = "Thuốc trừ sâu an toàn cho môi trường" },
                        new Product { ProductName = "Thuốc Trừ Sâu Y", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "200 ml", UnitPrice = 30000m, UnitsInStock = 147, UnitsOnOrder = 23, Discontinued = false, ProductDescription = "Thuốc trừ sâu cho cây ăn quả" },
                        new Product { ProductName = "Công Cụ Cuốc", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 80000m, UnitsInStock = 60, UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Cuốc làm đất bền chắc" },
                        new Product { ProductName = "Công Cụ Xẻng", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 70000m, UnitsInStock = 80, UnitsOnOrder = 20, Discontinued = false, ProductDescription = "Xẻng đào đất tiện lợi cho làm vườn" },
                        new Product { ProductName = "Phân Hữu Cơ", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "25 kg", UnitPrice = 55000m, UnitsInStock = 130, UnitsOnOrder = 25, Discontinued = false, ProductDescription = "Phân hữu cơ vi sinh cho đất tơi xốp" },
                        new Product { ProductName = "Hạt Giống Cà Chua", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 12000m, UnitsInStock = 210, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Hạt giống cà chua dễ trồng, quả to" },
                        new Product { ProductName = "Hạt Giống Cải Ngọt", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 11000m, UnitsInStock = 230, UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Hạt giống cải ngọt cho năng suất cao" },
                        new Product { ProductName = "Thuốc Diệt Cỏ", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "500 ml", UnitPrice = 45000m, UnitsInStock = 90, UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Thuốc diệt cỏ an toàn, không ảnh hưởng môi trường" },
                        new Product { ProductName = "Cào Làm Vườn", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 cái", UnitPrice = 40000m, UnitsInStock = 50, UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Cào làm vườn tiện dụng cho đất" },
                        new Product { ProductName = "Phân NPK", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "25 kg", UnitPrice = 75000m, UnitsInStock = 157, UnitsOnOrder = 21, Discontinued = false, ProductDescription = "Phân bón tổng hợp NPK chất lượng cao" },
                        new Product { ProductName = "Phân Lân", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "50 kg", UnitPrice = 65000m, UnitsInStock = 145, UnitsOnOrder = 30, Discontinued = false, ProductDescription = "Phân lân giúp cây phát triển rễ khỏe" },
                        new Product { ProductName = "Hạt Giống Dưa Leo", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "50 g", UnitPrice = 13000m, UnitsInStock = 120, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Hạt giống dưa leo phát triển nhanh" },
                        new Product { ProductName = "Hạt Giống Bắp Cải", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "50 g", UnitPrice = 14000m, UnitsInStock = 140, UnitsOnOrder = 9, Discontinued = false, ProductDescription = "Hạt giống bắp cải chất lượng cao" },
                        new Product { ProductName = "Thuốc Trừ Bệnh Lá", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "250 ml", UnitPrice = 35000m, UnitsInStock = 85, UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Thuốc trừ bệnh lá hiệu quả" },
                        new Product { ProductName = "Phân Bón Vi Sinh", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "10 kg", UnitPrice = 40000m, UnitsInStock = 110, UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Phân bón vi sinh cải tạo đất" },
                        new Product { ProductName = "Hạt Giống Xà Lách", CategoryID = 2, SupplierID = 2, QuantityPerUnit = "100 g", UnitPrice = 10000m, UnitsInStock = 250, UnitsOnOrder = 14, Discontinued = false, ProductDescription = "Hạt giống xà lách chất lượng" }
                    };
            context.Products.AddRange(products);
            context.SaveChanges();
            var images = new List<Image>();
            for (int i = 1; i <= 21; i++)
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
