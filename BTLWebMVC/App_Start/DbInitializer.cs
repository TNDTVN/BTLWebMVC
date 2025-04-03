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
                        IsLock = false,
                        TokenCode = null
                        },
                        new Account {Username = "customer",
                        Password = "customer123",
                        Email = "customer@vtnn.com",
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        Role = "Customer",
                        IsLock = false,
                        TokenCode= null
                        },
                        new Account {Username = "employee",
                        Password = "employee123",
                        Email = "employee@vtnn.com",
                        ProfileImage = "profile.jpg",
                        CreatedDate = DateTime.Now,
                        IsLock= false,
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
                // Linh Kiện máy tính
                //CPU Bộ vi xử lý 1 - 10
                        new Product { ProductName = "CPU Intel Core i5-12400F (Up To 4.40GHz, 6 Nhân 12 Luồng,18MB Cache, Alder Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1590000m, UnitsInStock = 100, 
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Được thiết kế để trở thành CPU cho game thủ, Core i5 12400F với bộ vi xử lí 6 nhân 12 luồng cho hiệu năng vượt trội hơn nhiều so với các sản phẩm tiền nhiệm. " +
                            "Mặc dù không có chip đồ họa tích hợp nhưng Core i5-12400F lại là một trong những chip Alder Lake đầu tiên sử dụng cấu trúc nhân Golden Cove hiệu suất cao để có sức mạnh ấn tượng." },
                        new Product { ProductName = "CPU Intel Core i9 14900K (Up 6.0 GHz, 24 Nhân 32 Luồng, 36MB Cache, Raptor Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 11490000m, UnitsInStock = 100, 
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "i9-14900K thể hiện sự vượt trội trong hầu hết các bài kiểm tra hiệu năng. Với khả năng ép xung lên đến 5.8 GHz, " +
                            "vi xử lý này cung cấp hiệu suất mạnh mẽ cho các tác vụ đòi hỏi tài nguyên cao như xử lý đồ họa, video, và chơi game. Các bài kiểm tra benchmark cho thấy i9-14900K vượt qua các phiên bản trước đó và nhiều đối thủ cạnh tranh trên thị trường." },
                        new Product { ProductName = "CPU Intel Core i7-12700K (Up To 5.00GHz, 12 Nhân 20 Luồng, 25M Cache, Alder Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 11000000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ vi xử lý Intel Core i7-12700K là một trong những sản phẩm nổi bật thuộc dòng Alder Lake của Intel, mang lại hiệu năng mạnh mẽ và nhiều cải tiến so với các thế hệ trước. " +
                            "Với 12 lõi và 20 luồng, i7-12700K hứa hẹn sẽ đáp ứng tốt mọi nhu cầu từ chơi game, làm việc sáng tạo đến các tác vụ đa nhiệm hàng ngày." },
                        new Product { ProductName = "CPU Intel Core i5-13400 (Up To 4.60GHz,10 Nhân 16 Luồng,20MB Cache, Raptor Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3290000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Intel Core i5-13400 là một trong những CPU - bộ vi xử lý thuộc dòng sản phẩm Intel Core thế hệ thứ 13 (Alder Lake). Đây là sản phẩm hướng đến đối tượng người dùng phổ thông, " +
                            "bao gồm cả những người làm việc văn phòng, game thủ và người dùng có nhu cầu xử lý đồ họa, biên tập video. Với sự cải tiến về kiến trúc và hiệu năng, Intel Core i5-13400 hứa hẹn mang lại trải nghiệm mượt mà và mạnh mẽ cho người dùng." },
                        new Product { ProductName = "CPU Intel Core i5-12400 (Up To 4.40GHz, 6 Nhân 12 Luồng,18MB Cache, Alder Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ vi xử lý Intel Core i5 12400 là một trong những CPU tầm trung đầu tiên của Intel được thiết kế dựa trên kiến trúc Alder Lake. Với mục tiêu phục vụ những người dùng yêu cầu " +
                            "hiệu năng cao hơn so với trung bình và có khả năng thực hiện tốt các tác vụ game phổ biến, Core i5 12400 là một sự lựa chọn tuyệt vời cho các bộ PC từ tầm trung đến cận cao cấp. Nó được trang bị các tính năng nổi trội của Intel gen 12 mới nhất, " +
                            "cho phép đạt hiệu năng tuyệt vời và mượt mà cho người dùng." },
                        new Product { ProductName = "CPU Intel Core i5 14600K (Up 5.3 GHz, 14 Nhân 20 Luồng, 24MB Cache, Raptor Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 6900000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ vi xử lý Intel Core i5-14600K là một trong những sản phẩm nổi bật trong dòng sản phẩm Core thế hệ thứ 14 của Intel. Với sự cải tiến về kiến trúc và hiệu năng, " +
                            "i5-14600K hứa hẹn mang lại trải nghiệm tuyệt vời cho người dùng từ làm việc, giải trí đến chơi game. " +
                            "Sản phẩm này hướng đến đối tượng người dùng yêu cầu hiệu năng cao và khả năng đa nhiệm tốt, đặc biệt là những game thủ và người làm việc với các ứng dụng đồ họa." },
                        new Product { ProductName = "CPU Intel Core Ultra 9 285K (Up 5.7 GHz, 24 Nhân 24 Luồng, 36MB Cache, Arrow Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 14590000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Intel Core Ultra 9 285K là một trong những vi xử lý mạnh mẽ nhất thuộc dòng Intel Core™ Ultra Processors (Series 2), được thiết kế cho máy tính bàn với hiệu năng tối ưu cho các tác vụ nặng như gaming, " +
                            "đồ họa, và xử lý đa nhiệm. Dòng Arrow Lake mang lại những cải tiến đáng kể, " +
                            "tiếp tục duy trì vị thế dẫn đầu của Intel trên thị trường CPU với nhiều công nghệ mới giúp tối ưu hóa hiệu suất và tiêu thụ năng lượng." },
                        new Product { ProductName = "CPU Intel Core i7-13700 (Up To 5.2 GHz, 16 Nhân 24 Luồng, 30M Cache, Raptor Lake)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 6690000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Intel Core i7-13700 là một trong những sản phẩm nổi bật của Intel, thuộc dòng Raptor Lake. Với mục tiêu cung cấp hiệu năng vượt trội cho các tác vụ đa nhiệm và chơi game, " +
                            "bộ vi xử lý này hứa hẹn mang đến trải nghiệm tuyệt vời cho người dùng. Bài viết này Hoàng Hà PC sẽ cung cấp " +
                            "cái nhìn chi tiết về thông số kỹ thuật, hiệu năng và các tính năng nổi bật của Intel Core i7-13700." },
                        new Product { ProductName = "CPU AMD Ryzen 9 7950X (Up To 5.7GHz, 16 Nhân 32 Luồng, 81MB Cache, AM5)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 11990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "AMD Ryzen 9 7950X là một trong những sản phẩm hàng đầu của AMD, được thiết kế nhằm mang lại hiệu năng mạnh mẽ cho các tác vụ nặng như chơi game, làm đồ họa, và các công việc chuyên nghiệp. " +
                            "Với những cải tiến vượt bậc so với các phiên bản trước, Ryzen 9 7950X hứa hẹn sẽ là lựa chọn " +
                            "hoàn hảo cho những người dùng đòi hỏi hiệu suất cao và ổn định." },
                        new Product { ProductName = "CPU AMD Ryzen 9 5900X (3.7GHz Turbo Up To 4.8GHz, 12 Nhân 24 Luồng, 70MB Cache, AM4)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 6490000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ vi xử lý AMD Ryzen 9 5900X chính là người kế thừa cho sản phẩm thành công nhất của AMD trong thị trường CPU trong những năm gần đây là AMD Ryzen 9 3900X. " +
                            "Vẫn giữ cho mình những hiệu năng cao cấp cũng như sở hữu cho mình những công nghệ mới giúp hiệu năng đơn nhân được cải thiện rõ rệt so với thế hệ trước kia." },
                // CPU Card đồ họa 11 - 20
                        new Product { ProductName = "Card màn hình VGA SAPPHIRE PURE AMD Radeon RX 9070 GPU", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 21900000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Card màn hình VGA SAPPHIRE PURE AMD Radeon RX 9070 GPU – Hiệu năng mạnh mẽ, trải nghiệm mượt mà VGA SAPPHIRE PURE AMD Radeon RX 9070 là một lựa chọn tuyệt vời cho game thủ và người dùng đồ họa chuyên nghiệp, " +
                            "mang đến hiệu suất mạnh mẽ nhờ kiến trúc AMD RDNA 4 tiên tiến. Với 16GB GDDR6, 256-bit , " +
                            "tốc độ 20 Gbps , cùng công nghệ tản nhiệt Tri-X Cooling , sản phẩm này đảm bảo trải nghiệm mượt mà, tối ưu trong mọi tác vụ. " },
                        new Product { ProductName = "Asus DUAL-RTX4060TI-8G-WHITE Card Đồ Họa GeForce RTX 4060 Ti Trắng Edition 8GB GDDR6 GPU Chơi Game Hiệu Suất Cao", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 12650000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "ASUS Dual GeForce RTX 4060 Ti Phiên bản Trắng 8GB GDDR6 với hai quạt Axial-tech mạnh mẽ và thiết kế 2.5-slot cho khả năng tương thích rộng rãi Được cung cấp bởi NVIDIA DLSS3 Tensor Cores thế hệ thứ 4: " +
                            "Lên tới 4x hiệu suất với DLSS 3 so với kết xuất brute-force RT Cores thế hệ thứ 3: Lên tới 2x hiệu" +
                            "suất ray tracing Thiết kế quạt Axial-tech có hub quạt nhỏ hơn giúp điều khiển cánh quạt dài hơn và vành rào tăng áp suất không khí xuống dưới. Một thiết kế 2.5-slot tối ưu hóa khả năng tương thích và hiệu suất làm mát cho hiệu suất vượt trội trong các khung máy nhỏ. " +
                            "Công nghệ 0dB cho phép bạn tận hưởng trải nghiệm chơi game nhẹ nhàng trong sự yên tĩnh tương đối. " +
                            "Vòng bi quạt đôi có thể kéo dài tuổi thọ gấp đôi so với thiết kế bi vỏ." },
                        new Product { ProductName = "VGA Nvidia Quadro M4000 8G GDDR5 256 bit", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 370000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Quadro M4000 là card đồ họa chuyên nghiệp của NVIDIA, Được xây dựng trên quy trình 28nm và dựa trên bộ xử lý đồ họa GM204, thẻ hỗ trợ DirectX 12.0. Bộ xử lý đồ họa GM204 là một con chip lớn với diện tích chết là 398 mm² " +
                            "và 5.200 triệu bóng bán dẫn. Không giống như GeForce GTX 980 đã được mở khóa hoàn toàn, " +
                            "sử dụng cùng một GPU nhưng đã bật tất cả 2048 shader (Cuda Core), NVIDIA đã vô hiệu hóa một số đơn vị đổ bóng trên Quadro M4000 để đạt được số lượng shader mục tiêu của sản phẩm." },
                        new Product { ProductName = "Gigabyte AORUS GV-N3080AORUS X-10GD card đồ họa NVIDIA GeForce RTX 3080 10 GB GDDR6X", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 10490000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Dòng bộ xử lý đồ họa: NVIDIA, bộ xử lý đồ họa: GeForce RTX 3080. Bộ nhớ card đồ họa rời: 10 GB, Kiểu bộ nhớ của card màn hình: GDDR6X, Bus bộ nhớ: 320 bit, Tốc độ xung nhịp bộ nhớ: 19000 MHz. Độ phân giải tối đa: 7680 x 4320 pixels. Phiên bản DirectX: 12.0, Phiên bản OpenGL: 4.6. Loại giao diện: PCI Express x16 4.0. " +
                            "Kiểu làm lạnh: Loa rời, Số lượng quạt: 3 quạt" },
                        new Product { ProductName = "Card đồ họa Asus Prime Radeon RX 9070 OC Edition 16GB (DDR6/ 256 bit)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 20300000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Card đồ họa Asus Prime Radeon RX 9070 OC Edition 16GB (DDR6/ 256 bit) Chip đồ họa: Radeon RX 9070 Bộ nhớ trong: 16Gb Kiểu bộ nhớ: DDR6 Tốc độ bộ nhớ: 20 Gbps Giao diện bộ nhớ: 256 bit Engine Clock: TBD DirectX: 12 Ultimate Chuẩn khe cắm: PCI Express 4.0 Cấu hình Card đồ họa Asus Prime Radeon RX 9070 OC Edition 16GB (DDR6/ 256 bit) " +
                            "Chip đồ họa Radeon RX 9070 Bộ nhớ trong 16Gb Kiểu bộ nhớ DDR6 Tốc độ bộ nhớ 20 Gbps Giao diện bộ nhớ 256 bit Engine Clock TBD DirectX 12 Ultimate Chuẩn khe cắm PCI Express 4.0 Cổng giao tiếp Yes x 1 (Native HDMI 2.1b) Yes x 3 (Native DisplayPort 2.1a) HDCP Support Yes (2.3) Công suất nguồn yêu cầu 550W Độ phân giải 7680 x 4320 Hỗ trợ hiển thị tối đa 4 Kích thước 312 x 130 x 50 mm 12.3 x 5.1 x 2.0 inch Phụ kiện kèm theo 1 x Speedsetup Manual" },
                        new Product { ProductName = "Gigabyte GV-N166SD6-6GD card đồ họa NVIDIA GeForce GTX 1660 SUPER 6 GB GDDR6", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 5490000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Gigabyte GV-N166SD6-6GD là card đồ họa thuộc dòng GeForce GTX 1660 SUPER của NVIDIA, được thiết kế để mang lại hiệu suất chơi game mạnh mẽ ở độ phân giải 1080p với tốc độ khung hình mượt mà. Sử dụng bộ nhớ GDDR6 6GB, nó nhanh hơn đáng kể so với phiên bản GTX 1660 thông thường." },
                        new Product { ProductName = "Card màn hình SAPPHIRE PURE AMD Radeon™ RX 9070 GPU", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 21100000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "SAPPHIRE PURE AMD Radeon™ RX 9070 là một card đồ họa hiệu năng cao, được thiết kế dành cho game thủ và người sáng tạo nội dung chuyên nghiệp. Với kiến trúc RDNA 3.5 tiên tiến, bộ nhớ 16GB GDDR6 tốc độ cao và giao diện PCIe 5.0, RX 9070 mang đến trải nghiệm gaming 4K mượt mà, khả năng xử lý đồ họa mạnh mẽ và hỗ trợ công nghệ FSR 4 giúp tối ưu hiệu suất. Hệ thống tản nhiệt ba quạt AeroCurve " +
                            "giúp duy trì nhiệt độ ổn định, đảm bảo card hoạt động êm ái ngay cả khi tải nặng. Đây là lựa chọn lý tưởng cho những ai đang tìm kiếm một GPU mạnh mẽ, bền bỉ và tiết kiệm năng lượng trong tầm giá!" },
                        new Product { ProductName = "Card màn hình VGA RTX 5090 32G VENTUS 3X OC", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 90990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sản phẩm được trang bị kiến trúc NVIDIA Blackwell mạnh mẽ cùng với công nghệ DLSS 4, mang đến khả năng xử lý đồ họa ấn tượng. Với 21.760 nhân CUDA và bộ nhớ GDDR7 32GB, card đồ họa này đáp ứng tốt nhu cầu chơi game ở độ phân giải 4K và 8K, cũng như các tác vụ đồ họa chuyên sâu như dựng phim và thiết kế 3D." },
                        new Product { ProductName = "MSI GeForce RTX 3060 GAMING X 12G NVIDIA 12 GB GDDR6", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 8990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Dòng bộ xử lý đồ họa: NVIDIA, bộ xử lý đồ họa: GeForce RTX 3060. Bộ nhớ card đồ họa rời: 12 GB, Kiểu bộ nhớ của card màn hình: GDDR6, Bus bộ nhớ: 192 bit. Độ phân giải tối đa: 7680 x 4320 pixels. Phiên bản DirectX: 12.0, Phiên bản OpenGL: 4.6. Loại giao diện: PCI Express 4.0. Kiểu làm lạnh: Loa rời, Số lượng quạt: 2 quạt, Màu của cường độ ánh sáng: Đa sắc" },
                        new Product { ProductName = "MSI RTX 3070 TI VENTUS 3X 8G OC card đồ họa NVIDIA GeForce RTX 3070 Ti 8 GB GDDR6X", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 16990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Dòng bộ xử lý đồ họa: NVIDIA, bộ xử lý đồ họa: GeForce RTX 3070 Ti. Bộ nhớ card đồ họa rời: 8 GB, Kiểu bộ nhớ của card màn hình: GDDR6X, Bus bộ nhớ: 256 bit. Độ phân giải tối đa: 7680 x 4320 pixels. Phiên bản DirectX: 12.0, Phiên bản OpenGL: 4.6. Loại giao diện: PCI Express 4.0. Kiểu làm lạnh: Loa rời, Số lượng quạt: 3 quạt" },
                // MainBoard 21 - 30
                        new Product { ProductName = "Mainboard ASUS PRIME H510M-F Prime QSD", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 980000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tenda AC8 , i5 12600k , A320M A Pro , amd athlon 3000g , 24MP400 B , CPU Intel Core i5 12400F , LS22A336NHEXXV , LG 27MK600M B , MSI Pro MP243 , ssd samsung 250gb 870 evo , Corsair Vengeance LPX 8GB 3200Mhz , core i5 10400 , màn hình máy tính cong , rtx 4090 , rtx 4080 , ram 8gb ddr4 , switch SG108 , Viewsonic VA1903H , SE2422H , ST1000VX005 , M99 , rx 6700 xt , " +
                            "Samsung 870 EVO MZ 77E500BW , 24GQ50F B , M7000 , ssd 128gb , Tenda N301 , LG 20MK400H B , Kingston Fury Beast 16GB DDR5 5600Mhz , rtx 3050ti , EM901X , TP Link TL SF1005D" },
                        new Product { ProductName = "Mainboard MSI A320M-A PRO – Socket AMD AM4", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1350000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Thiết kế: Nhỏ gọn, phù hợp với nhiều loại máy khác nhau.Hiệu năng: Mạnh mẽ, sử dụng CPU thế hệ mới.Tính năng đi kèm: Có thể điều chỉnh hoạt động của máy trên MSI Command Center.Giá bán: [price].Nếu bạn đang tìm kiếm một bo vi mạch thuộc phân khúc tầm trung, nhưng cung cấp đủ tính năng cần thiết thì Mainboard MSI A320M-A PRO - Socket AMD AM4 là một sự lựa chọn đáng thử trong thời gian tới." },
                        new Product { ProductName = "Mainboard MSI B360M MORTAR", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1450000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Mainboard MSI B360M MORTAR Mainboard MSI B360M MORTAR là bo mạch chủ tầm trung của MSI sử dụng chipset Intel B360. Tính năng Mainboard MSI B360M MORTAR Hỗ trợ bộ vi xử lý Intel thế hệ thứ 8th Gen Intel Core / Pentium / Celeron cho socket LGA 1151 Hỗ trợ bộ nhớ DDR4-2666 MHz Audio Boost 4: Mang lại âm thanh phòng thu với chip xử lý âm thanh cao cấp mang lại cho bạn trải nghiệm game ấn tượng. " +
                            "DDR4 Boost : Công nghệ cao cấp từ phòng lab MSI OC bảo đảo sự tương thích mang lại hiệu năng ép xung cao nhất. Intel LAN:Mang lại trải nghiệm game mượt mà giảm tải cho CPU, giúp CPU tập trung giải quyết những tác vụ khác." },
                        new Product { ProductName = "Mainboard ASUS PRIME B450M-A II", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1690000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "ainboard ASUS PRIME B450M-A II Bo mạch chủ AMD B450 (Ryzen AM4) micro ATX với hỗ trợ M.2, hỗ trợ HDMI/DVI-D/D-Sub, SATA 6 Gbps, 1 Gb Ethernet, USB 3.2 Gen 2 Type-A, BIOS FlashBack và hỗ trợ chiếu sáng Aura Sync RGB Làm mát toàn diện: Vrm tản nhiệt, tản nhiệt PCH, và Fan Xpert 2 + Kết nối siêu nhanh: Hỗ trợ M.2, Ethernet 1 Gb và USB 3.2 Gen 2 Type-A 5X Bảo vệ III: Nhiều biện pháp bảo vệ phần cứng " +
                            "để bảo vệ toàn bộ hệ thống Aura Sync RGB chiếu sáng: Onboard kết nối cho RGB LED dải, dễ dàng đồng bộ hóa với một danh mục đầu tư ngày càng tăng của Aura Sync. " },
                        new Product { ProductName = "Mainboard Asrock B450M HDV DDR4", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1350000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Mainboard Asrock B450M HDV DDR4 Thiết kế tiện dụng Bo Mạch Chủ Mainboard ASRock B450M HDV Socket AM4 được tạo ra để có thể hỗ trợ tốt nhất cho các dòng chip AMD sử dụng socket AM4 mang lại khả năng điều khiển chính xác và sức mạnh mượt mà cung cấp năng lượng sạch và hiệu quả đảm bảo cho CPU hoạt động ổn định và tuổi thọ của bo mạch chủ cũng được tăng lên đáng kể.. Công nghệ AMD StoreMI Một công cụ mạnh " +
                            "mẽ kết hợp tốc độ ổ SSD của bạn với dung lượng ổ cứng của bạn thành một ổ đĩa đơn, nhanh, dễ quản lý. SSD nhanh, nhưng đắt tiền và cung cấp dung lượng tối thiểu. Ổ cứng cơ học tự hào có dung lượng lớn với giá thấp, nhưng chậm hơn nhiều so với SSD. Công nghệ AMD StoreMI kết hợp giữa hai loại lưu trữ này vào một ổ đĩa và tự động di chuyển dữ liệu bạn truy cập nhiều nhất vào SSD, do đó bạn có thể tận dụng tốt nhất cả hai thế giới: khả năng phản hồi của SSD và dung lượng ổ cứng cơ. " },
                        new Product { ProductName = "Mainboard Gigabyte B550M Aorus Elite - MSI B550m Mortar (AMD B550, Socket AM4, m-ATX, 4 khe RAM DDR4)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2640000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "GIGABYTE B550M Aorus Elite - MSI B550m Mortar : + Kích thước: M-ATX + Socket: AM4 + Chipset: B550 + Khe RAM tối đa: 4 + Loại RAM hỗ trợ: DDR4 ƯU ĐIỂM SẢN PHẨM - Được trang bị chipset B550, bo mạch chủ này hỗ trợ các vi xử lý AMD Ryzen thế hệ 3000 và 5000, mang lại hiệu suất vượt trội và tính năng mở rộng tốt. - Với Socket AM4, bo mạch chủ tương thích với các dòng vi xử lý AMD Ryzen và Athlon, dễ dàng nâng cấp hoặc thay thế CPU. " +
                            "Hỗ trợ tối đa 64GB RAM DDR4, mang lại khả năng mở rộng cao cho các tác vụ đa nhiệm và các ứng dụng yêu cầu bộ nhớ lớn. " },
                        new Product { ProductName = "Mainboard PC MSI B450M-A PRO MAX II", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1550000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Mainboard PC MSI B450M-A PRO MAX II là sự lựa chọn hoàn hảo cho những ai đang tìm kiếm một nền tảng vững chắc để xây dựng hệ thống PC mạnh mẽ và ổn định. Với thiết kế tối ưu và nhiều tính năng vượt trội, sản phẩm này sẽ đáp ứng mọi nhu cầu của bạn, từ làm việc, học tập đến giải trí và chơi game." },
                        new Product { ProductName = "Bo Mạch Chủ Mainboard ASUS PRIME B840M-A-CSM", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 5190000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bo mạch chủ Mainboard ASUS PRIME B840M-A-CSM là một sản phẩm đột phá trong dòng bo mạch chủ micro-ATX, được thiết kế đặc biệt để tương thích với các bộ vi xử lý AMD Ryzen™ 9000, 8000 và 7000 Series. Mainboard B840M-A-CSM không chỉ đáp ứng yêu cầu về hiệu suất mà còn mang lại những tính năng vượt trội phù hợp cho các game thủ, nhà sáng tạo nội dung và người dùng chuyên nghiệp." },
                        new Product { ProductName = "Mainboard PC MSI PRO H610M-E DDR4", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1950000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bo mạch chủ PRO Series giúp người dùng làm việc thông minh hơn bằng cách mang lại trải nghiệm hiệu quả và năng suất ổn định, với linh kiện lắp ráp chất lượng cao, bo mạch chủ PRO Series không chỉ cung cấp quy trình làm việc chuyên nghiệp được tối ưu hóa mà còn bảo vệ để ít gặp sự cố cùng kéo dài tuổi thọ." },
                        new Product { ProductName = "Mainboard Gigabyte B860M EAGLE WIFI6", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3850000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Mainboard GIGABYTE B860M EAGLE WIFI6 hỗ trợ CPU Intel thế hệ mới, trang bị WiFi 6, PCIe 4.0, RAM DDR5 và nhiều cổng kết nối hiện đại. Hiệu suất mạnh mẽ, lý tưởng cho gaming và làm việc." },
                // PSU 31 - 40
                        new Product { ProductName = "PSU MSI MAG A650BN 650W - 80 Plus Bronze", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1349000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tổng công suất: 650 W, Điện áp AC đầu vào: 100 - 240 V, Tần số AC đầu vào: 50/60 Hz. Motherboard power connector: 20+4 pin ATX, Chiều dài dây nguồn bo mạch chủ: 60 cm, Loại dây cáp: Mô đun. Mục đích: Máy tính cá nhân, Hệ số hình dạng bộ nguồn máy tính (PSU): ATX, Chứng nhận 80 PLUS: 80 PLUS Bronze. Màu sắc sản phẩm: Màu đen, Kiểu làm lạnh: Loa rời, Đường kính quạt: 12 cm. Chiều rộng: 150 mm, Độ dày: 140 mm" },
                        new Product { ProductName = "Nguồn ASUS ROG LOKI 850W 80 Plus Platinum Fully Modullar SFX-L", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4490000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn ASUS ROG LOKI 850W 80 Plus Platinum Fully Modullar SFX-L là một trong những sản phẩm nổi bật trong dòng sản phẩm nguồn máy tính cao cấp. Được thiết kế đặc biệt cho các game thủ và những người yêu công nghệ, ASUS ROG LOKI 850W 80 Plus mang lại hiệu suất cực kỳ ổn định và đáng tin cậy. Với tiêu chuẩn đánh giá hiệu suất 80 Plus Platinum, nguồn ASUS ROG LOKI 850W không chỉ tiết kiệm điện mà còn đảm bảo rằng hệ thống của bạn hoạt động mượt mà dưới mọi điều kiện tải." },
                        new Product { ProductName = "Cooler Master MWE BRONZE V3 230V 650W A/EU Cable (80 Plus Bronze/ ATX/ Đen)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1290000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Cấu hình Nguồn máy tính Cooler Master MWE BRONZE V3 230V 650W A/EU Cable (80 Plus Bronze/ ATX/ Đen) Công suất danh định 650W Chuẩn nguồn 80 Plus Bronze Đầu cấp điện cho main 204pins Đầu cấp điện cho hệ thống 1 x ATX 24-PIN 2 x EPS 8 / 44 Pin 6 x SATA 3 x Peripheral 4 Pin 4 x PCI-e 62 Pin Quạt làm mát Fan 12cm Điện áp vào 200-240V Kiểu cáp nguồn Non-Modular (Dây liền toàn bộ) Kích cỡ nguồn ATX Màu sắc Đen" },
                        new Product { ProductName = "Asus TUF Gaming 1200W Vàng", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4490000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Được Xây Dựng Để Bền Bỉ ASUS TUF Gaming 1200W Gold tích hợp sức mạnh quân sự, thiết kế quạt hàng đầu và bảo vệ PCB để cung cấp năng lượng hiệu quả và bền bỉ. Kết quả là một bộ nguồn PSU sẽ cung cấp năng lượng cho hệ thống của bạn một cách đáng tin cậy trong nhiều năm tới. Bền Bỉ | Thành Phần TUF | Điểm số hàng đầu Với các linh kiện quân sự, bao gồm tụ điện được xếp hạng cho hoạt động ở 105°C và tuổi thọ lên tới 170.000 giờ, ASUS TUF Gaming 1200W Gold cho phép bạn tự tin xây dựng." },
                        new Product { ProductName = "Nguồn FSP DAGGER PRO ATX3.0 PCIe5.0 850W SFX 80 Plus Gold Fully-Modular", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3250000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn máy tính FSP Dagger Pro ATX3.0 PCIe5.0 850W là một trong những sản phẩm hàng đầu trong lĩnh vực cung cấp năng lượng cho hệ thống máy tính hiện đại.\r\n\r\nVới công suất lên đến 850W, FSP Dagger Pro ATX3.0 đáp ứng tốt nhu cầu của các game thủ và người dùng chuyên nghiệp, mang lại hiệu suất ổn định và bền bỉ. Thiết kế nhỏ gọn cùng với chuẩn ATX3.0 và PCIe5.0 giúp nguồn có thể tương thích dễ dàng với nhiều linh kiện cao cấp, đảm bảo quá trình hoạt động tối ưu nhất." },
                        new Product { ProductName = "Nguồn máy tính Corsair RM850e PCIE5 850W 80 Plus Gold CP-9020263-NA", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2750000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn máy tính Corsair RM850e PCIE5 850W 80 Plus Gold được trang bị đầu nối 16 PIN để tương thích PCIe 5.0 và ATX 3.0. Nhờ có ATX 3.0, bộ nguồn cũng có thể nâng mức cung cấp tiêu thụ năng lượng lên gấp 2 lần và gấp 3 lần mức tiêu thụ năng lượng GPU." },
                        new Product { ProductName = "Nguồn Máy Tính Corsair RM1000e 2025 ATX 3.1 80 Plus Gold - Full Modular", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4590000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn máy tính Corsair RM1000e 2025 ATX 3.1 80 Plus Gold - Full Modular là một trong những sản phẩm nổi bật trong dòng nguồn cấp điện cho máy tính hiện nay. Với công suất lên đến 1000W, Corsair RM1000e không chỉ đáp ứng tốt nhu cầu cho các hệ thống máy tính mạnh mẽ mà còn thích hợp cho các game thủ, người dựng phim và thiết kế đồ họa. Được chứng nhận 80 Plus Gold, nguồn Corsair đảm bảo hiệu suất năng lượng tối ưu, giúp tiết kiệm điện và giảm thiểu lượng nhiệt phát sinh trong quá trình sử dụng." },
                        new Product { ProductName = "Nguồn máy tính FSP Dagger Pro 850W Gen 5 80 Plus Gold SFX Full Modular", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3190000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn máy tính FSP Dagger Pro 850W Gen 5 80 Plus Gold SFX Full Modular là sự lựa chọn tối ưu cho những dàn máy cao cấp với công suất lên đến 850W và chứng nhận 80Plus Gold, giúp tiết kiệm điện năng và tối ưu hóa hiệu suất hoạt động. Sản phẩm mang lại khả năng cung cấp điện ổn định và đáng tin cậy, giúp máy tính của bạn hoạt động trơn tru và đạt hiệu suất cao nhất." },
                        new Product { ProductName = "Nguồn Asus TUF-GAMING 750W Gaming Bronze", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1550000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn ASUS TUF Gaming 750W Bronze được thiết kế để đảm bảo sự bền bỉ và độ tin cậy cao, với chứng nhận tiêu chuẩn quân sự. Sản phẩm kết hợp các linh kiện chất lượng cao cấp quân sự cùng với giải pháp làm mát hiệu quả, tạo ra một nguồn năng lượng vững chắc mà bạn có thể hoàn toàn yên tâm sử dụng cho hệ thống máy tính của mình." },
                        new Product { ProductName = "Nguồn ASUS ROG LOKI 1000W 80 Plus Platinum Fully Modullar SFX-L", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1890000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nguồn ASUS ROG LOKI 1000W 80 Plus Platinum Fully Modullar SFX-L là một sản phẩm đáng chú ý trong danh mục nguồn cao cấp. Được thiết kế với hiệu suất 80 Plus Platinum, ASUS ROG LOKI 1000W 80 Plus Platinum không chỉ cung cấp nguồn điện mạnh mẽ mà còn tối ưu hóa hiệu suất năng lượng, giúp tiết kiệm chi phí điện năng cho người sử dụng. Với công suất lên đến 1000W, nguồn ASUS ROG LOKI hoàn toàn đáp ứng được nhu cầu của các game thủ chuyên nghiệp và người làm việc trong lĩnh vực đồ họa." },
                // Ram 41 - 50
                        new Product { ProductName = "Ram PC máy tính bàn Kingston DDR4 4GB, 8GB, 16GB Bus 3200, 2400, 2666Mhz 1600 ", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "DDR4 - 8GB - Bus 2666MHz - PC4 21300 Kingston 8GB DDR4-2666 là bộ nhớ RAM chuẩn DDR4 có tốc độ xử lí đạt 2666MHz giúp tối ưu hiệu năng cho bộ xử lý CPU đến từ cả Intel lẫn AMD. Thuộc dòng sản phẩm phần cứng của thương hiệu nổi tiếng Kingston giúp bạn yên tâm hơn khi sử dụng. Đặc biệt, RAM được làm từ chất liệu cao cấp nên có tuổi thọ lâu dài và đảm bảo độ bền khi sử dụng liên tục trong thời gian dài." },
                        new Product { ProductName = "Ram PC HIKSEMI ARMOR 8GB (1X8GB) DDR4 3200MHZ", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 450000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ nhớ RAM PC Hiksemi Armor 8GB (1x8GB) DDR4 3200MHz là một lựa chọn lý tưởng cho những ai đang tìm kiếm hiệu suất tối ưu cho máy tính của mình. Với dung lượng 8GB, Hiksemi Armor cung cấp khả năng xử lý đa nhiệm mượt mà và nhanh chóng. Với tốc độ 3200MHz, RAM Hiksemi Armor đảm bảo thời gian phản hồi nhanh chóng cho các tác vụ đòi hỏi hiệu suất cao." },
                        new Product { ProductName = "Ram Máy Tính Patriot 8GB DDR4 3200MHz", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 600000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Với dung lượng 8GB, RAM Patriot giúp bạn có thể chạy hiệu quả đa nhiệm các ứng dụng, hỗ trợ bạn thuận tiện hơn, dễ dàng hơn với công việc. RAM Desktop Patriot 8GB DDR4 3200MHz có chuẩn bộ nhớ DDR4 kết hợp độ Bus lên đến 3200MHz không chỉ mang lại hiệu năng hoạt động mạnh mẽ cho máy tính, mà còn giúp tiêu hao ít năng lượng hơn 30% so với chuẩn DDR3, từ đó bảo vệ và duy trì được tuổi thọ hoạt động cho máy tính của bạn." },
                        new Product { ProductName = "RAM Desktop Kingston 8GB DDR4 2666 MHz NB", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 840000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Các sản phẩm bộ nhớ RAM của Kingston được nghiên cứu, thiết kế và kiểm nghiệm riêng cho laptop, nhằm đảm bảo khả năng tương thích, tính ổn định cũng như hiệu năng tối ưu nhất tới người sử dụng laptop. Tất cả các dòng sản phẩm bộ nhớ RAM của Kingston trước khi đến tới tay người dùng đều phải được trải qua công đoạn kiểm nghiệm nghiêm ngặt từ nhà máy sản xuất nhằm hạn chế gần như tuyệt đối các sự cố có thể xảy ra trong khi hoạt động." },
                        new Product { ProductName = "Ram PC Corsair Vengeance LPX 8GB 3200MHz DDR4 CMK8GX4M1E3200C16", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 420000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ram Corsair được thiết kế sản xuất và thử nghiệm cực kì chặt chẽ để đảm bảo hiệu suất và khả năng tương thích hoàn hảo nhất trên hầu hết tất cả các bo mạch chủ x99 Series trên thị trường. Bo mạch chủ Intel x99 dựa trên chuẩn XMP 2.0 mới, và Ram Vengeance LPX DRAM hiện tại cũng đã tương thích hoàn toàn. Chỉ cần bật chức năng ép xung, và nó sẽ tự động điều chỉnh tốc độ nhanh nhất và an toàn nhất cho bộ Vengeance LPX kit của bạn. Bạn sẽ nhận được hiệu suất tuyệt vời mà không cần lo lắng vấn đề gì cả." },
                        new Product { ProductName = "Ram Pc Gskill 8GB DDR4 (3200Mhz | Tản Nhiệt Lá | FullBox) F4-3200C16S-8GIS", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 450000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Được đặt tên theo sau lá chắn mạnh mẽ của các vị thần Hy Lạp, Aegis tượng trưng cho sức mạnh và sức mạnh. Sự bổ sung mới này của bộ nhớ DDR4 vào bộ nhớ game dành cho gia đình của AEGIS được thiết kế để nâng cao hiệu năng và sự ổn định cao trên các hệ thống chơi game mới nhất của PC. Cùng dung lượng 8GB với tốc độ bus 3200MHz Sản phẩm này chắc chắn sẽ giúp máy của bạn tăng tốc trong việc cài đặt ứng dụng, duyệt web hoặc chơi các trò game đòi hỏi cấu hình cao." },
                        new Product { ProductName = "RAM Desktop Kingston 8GB DDR4 2400MHz NB", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 965000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Với nhiều năm kinh nghiệm sản xuất thiết bị lưu trữ và bộ nhớ cho máy tính, các sản phẩm của Kingston luông nhận được rất nhiều sự tin tưởng từ cả người dùng cá nhân cho tới các doanh nghiệp lớn nhờ vào khả năng hoạt động cực kì ổn định kèm theo mức giá hợp lý.\r\n\r\nCác sản phẩm bộ nhớ RAM của Kingston được nghiên cứu, thiết kế và kiểm nghiệm riêng cho laptop, nhằm đảm bảo khả năng tương thích, tính ổn định cũng như hiệu năng tối ưu nhất tới người sử dụng laptop. Tất cả các dòng sản phẩm bộ nhớ RAM của Kingston trước khi đến tới tay người dùng đều phải được trải qua công đoạn kiểm nghiệm nghiêm ngặt từ nhà máy sản xuất nhằm hạn chế gần như tuyệt đối các sự cố có thể xảy ra trong khi hoạt động." },
                        new Product { ProductName = "Ram PC GSKILL RIPJAWS DDR3 4GB", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 150000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "RAM GSKILL RIPJAWS 4GB DDR3 / BUS 1600 được thiết kế với công nghệ tiên tiến và đạt chuẩn xử lý tốc độ cao. Khả năng tự động tối ưu hóa hiệu suất của máy tính của bạn và đảm bảo rằng bạn có thể sử dụng máy tính của mình một cách hiệu quả nhất.\r\n\r\nRAM GSKILL RIPJAWS 4GB DDR3 / BUS 1600 cũng được trang bị với một số tính năng bảo mật như chức năng bảo vệ dữ liệu, bảo vệ độ ổn định, và các tính năng bảo mật khác. Nó cũng có thể được sử dụng để nâng cấp máy tính của bạn và đảm bảo rằng nó hoạt động ổn định và hiệu quả." },
                        new Product { ProductName = "Ram Laptop Corsair Vengeance DDR4 16GB 3200MHz 1.2v CMSX16GX4M1A3200C22", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 740000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ nâng cấp Ram Corsair Vengeance dành cho Laptop là sản phẩm cao cấp được thiết kế cho Notebook và Laptop. Ram Corsair Vengeace được làm từ các chip nhớ DRAM hiệu năng cao đã được chọn lọc và đáng tin cậy. Ram Corsair Vengeance với độ trễ thấp mang lại một tốc độ đáp ứng gần như tức thì cho mọi ứng dụng của bạn." },
                        new Product { ProductName = "Ram Corsair Vengeance RGB RS DDR4 3200MHZ 32GB (2x16GB) ", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2150000m, UnitsInStock =
                            100, UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ram Corsair Vengeance RGB RS DDR4 3200MHZ 32GB với hệ thống chiếu sáng máy tính của bạn với ánh sáng RGB cực kỳ hầm hố, đồng thời mang lại hiệu suất cao nhất của DDR4. Phần mềm CORSAIR iCUE mạnh mẽ giúp hệ thống của bạn trở nên sống động với khả năng tinh chỉnh RGB năng động, phần mềm này cũng có thể đồng bộ trên tất cả các sản phẩm tương thích iCUE bao gồm bộ nhớ, quạt, dải đèn LED RGB, bàn phím, chuột và nhiều hơn nữa. Bạn cũng có thể lưu nhiều Profile cùng một lúc để thay đổi nhanh chóng và dễ dàng hơn." },
                // SSD 51 - 60
                        new Product { ProductName = "SSD Kingston A400 120GB SATA III", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 450000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "SSD Kingston A400 120GB SATA III là một trong những sản phẩm ổ cứng SSD giá rẻ nhưng hiệu quả nhất hiện nay. Với dung lượng 120GB, Kingston A400 cung cấp không gian lưu trữ đủ lớn cho hệ điều hành và các ứng dụng cơ bản. Tốc độ đọc ghi dữ liệu lên đến 500MB/s giúp máy tính khởi động nhanh chóng và ứng dụng chạy mượt mà." },
                        new Product { ProductName = "SSD Kingston A2000 500GB NVMe PCIe Gen 3.0 x4", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1250000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "SSD Kingston A2000 500GB NVMe PCIe Gen 3.0 x4 là một trong những sản phẩm ổ cứng SSD NVMe giá rẻ nhưng hiệu quả nhất hiện nay. Với dung lượng 500GB, Kingston A2000 cung cấp không gian lưu trữ đủ lớn cho hệ điều hành và các ứng dụng cơ bản. Tốc độ đọc ghi dữ liệu lên đến 2000MB/s giúp máy tính khởi động nhanh chóng và ứng dụng chạy mượt mà." },
                        new Product { ProductName = "SSD Kingston KC600 1TB SATA III", CategoryID = 1, Category = categories[0], SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2680000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "SSD Kingston KC600 1TB SATA III là một trong những sản phẩm ổ cứng SSD cao cấp nhất hiện nay. Với dung lượng 1TB, Kingston KC600 cung cấp không gian lưu trữ đủ lớn cho hệ điều hành và các ứng dụng cơ bản. Tốc độ đọc ghi dữ liệu lên đến 550MB/s giúp máy tính khởi động nhanh chóng và ứng dụng chạy mượt mà." },

                        new Product { ProductName = "Ổ Cứng SSD Kingston KC2500 2TB NVMe M.2 PCIe Gen 3 x4 (SKC2500M8/2000G)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4880000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ổ cứng SSD Kingston KC2500 M.2 PCIe Gen3 x4 NVMe của Kingston là giải pháp nâng cấp máy tính để mang đến hiệu năng siêu cao khi được trang bị chuẩn NVMe PCIe Gen 3 x4, chip điều khiển Controller SMI 2262EN kèm với công nghệ 3D-NAND TLC 96-Layers để mang đến hiệu năng cực cao với tốc độ truyền dữ liệu lên đến 3500MB/s. KC2000 cho độ bền cao và giúp tăng hiệu suất trên máy bàn, máy trạm và các hệ thống High Performance Computing (HPC)." },
                        new Product { ProductName = "Ổ Cứng SSD Kingston KC2500 1TB NVMe M.2 PCIe Gen 3 x4 (SKC2500M8/1000G)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3150000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ổ cứng SSD Kingston KC2500 M.2 PCIe Gen3 x4 NVMe của Kingston là giải pháp nâng cấp máy tính để mang đến hiệu năng siêu cao khi được trang bị chuẩn NVMe PCIe Gen 3 x4, chip điều khiển Controller SMI 2262EN kèm với công nghệ 3D-NAND TLC 96-Layers để mang đến hiệu năng cực cao với tốc độ truyền dữ liệu lên đến 3500MB/s. KC2000 cho độ bền cao và giúp tăng hiệu suất trên máy bàn, máy trạm và các hệ thống High Performance Computing (HPC)." },

                        new Product { ProductName = "Ổ cứng SSD 3.84TB Enterprise Kingston DC450R", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4965000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ổ cứng SSD 3.84TB Enterprise Kingston DC450R là một sản phẩm lưu trữ cao cấp được thiết kế đặc biệt cho các trung tâm dữ liệu với nhu cầu đọc dữ liệu chuyên sâu. Thuộc dòng SSD doanh nghiệp của Kingston, DC450R sử dụng công nghệ 3D TLC NAND, mang đến dung lượng lớn trong form factor 2.5 inch, cùng hiệu suất vượt trội với tốc độ đọc lên đến 560MB/s và tốc độ ghi tối đa 530MB/s. Được tối ưu hóa cho các ứng dụng như mạng phân phối nội dung (CDN), điện toán biên (edge computing) và kiến trúc lưu trữ định nghĩa bằng phần mềm (SDS), ổ cứng này đảm bảo độ trễ thấp (dưới 500µs khi đọc) và hiệu suất ổn định nhờ các yêu cầu QoS nghiêm ngặt. Với độ bền ấn tượng lên đến 2.823TBW và bảo hành 5 năm, Kingston DC450R là lựa chọn lý tưởng cho các doanh nghiệp cần giải pháp lưu trữ đáng tin cậy, hiệu quả và tiết kiệm chi phí." },
                        new Product { ProductName = "Ổ cứng SSD 3.84TB Enterprise Kingston DC450R (2.5\", SATA III)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 14868000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Đáp ứng nhu cầu thời gian hoạt động 24/7 và độ tin cậy cho doanh nghiệp của bạn. DC450R của Kingston trình làng một bộ tính năng tập trung đặc biệt cho phép các trung tâm dữ liệu chọn lựa SSD hiệu quả nhất về mặt chi phí cho khối lượng công việc của mình. Các doanh nghiệp cần có được kết quả khi họ cung cấp các sản phẩm, giải pháp và thỏa thuận cấp dịch vụ (SLA). DC450R của Kingston được thiết kế để đáp ứng những kỳ vọng đó." },
                        new Product { ProductName = "Ô cứng SSD 2TB Crucial P3 Plus (NVME, 4700MB/s, Gen4x4)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2690000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ổ cứng SSD 2TB Crucial P3 Plus (NVMe, 4700MB/s, Gen4x4) là một sản phẩm lưu trữ hiệu suất cao thuộc dòng SSD PCIe Gen4 của Crucial, được thiết kế để đáp ứng nhu cầu của người dùng phổ thông và game thủ với mức giá hợp lý. Sử dụng công nghệ 3D NAND QLC 176 lớp của Micron, ổ cứng này cung cấp dung lượng lớn 2TB trong form factor M.2 2280, phù hợp cho cả laptop và desktop hỗ trợ PCIe 4.0." },
                        new Product { ProductName = "Ổ cứng SSD 4TB Kingston KC3000 (M.2 PCIe Gen4 x4 NVMe)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 9950000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Kingston KC3000 PCIe 4.0 NVMe M.2 SSD mang lại cho bạn hiệu năng vượt trội nhờ bộ điều khiển NVMe Gen 4x4 mới nhất và 3D TLC NAND. Nâng cấp bộ nhớ và nâng cao độ tin cậy cho hệ thống để đáp ứng khối lượng công việc với mức độ đòi hỏi khắt khe, đồng thời mang đến hiệu năng tốt hơn trên các ứng dụng phần mềm như kết xuất 3D và tạo nội dung 4K+. Với tốc độ đọc/ghi ấn tượng lên đến 7.000MB/giây, ổ cứng này chắc chắn sẽ cải thiện quy trình làm việc trên máy tính để bàn và máy tính xách tay hiệu năng cao, trở thành lựa chọn lý tưởng cho những người dùng thành thạo cần tốc độ nhanh nhất trên thị trường." },
                        new Product { ProductName = "Ổ cứng SSD MSI Spatium M450 500GB | PCIe 4.0 NVMe M.2 PCI Express 4.0", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 990000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Ổ cứng được trang bị công nghệ 3D NAND flash, giúp tăng mật độ lưu trữ trong thiết kế nhỏ gọn, đồng thời tích hợp các tính năng bảo vệ dữ liệu và sửa lỗi (ECC) để nâng cao độ bền và tuổi thọ. Độ bền của phiên bản 500GB được đánh giá ở mức 300TBW (Terabytes Written), phù hợp cho các tác vụ thông thường như chơi game, làm việc văn phòng hoặc lưu trữ dữ liệu cá nhân. Tuy nhiên, do thiết kế không có DRAM (DRAM-less) và sử dụng bộ điều khiển Phison E19T, hiệu suất ghi có thể giảm khi thực hiện các tác vụ nặng liên tục sau khi bộ đệm bị đầy, khiến nó kém tối ưu hơn cho các ứng dụng như chỉnh sửa video chuyên sâu." },
                // Tản nhiệt 61 - 70 
                        new Product { ProductName = "Tản nhiệt CPU Cooler Master Hyper 212 RGB Black Edition", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1070000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tản nhiệt CPU Cooler Master Hyper 212 RGB Black Edition là một trong những sản phẩm tản nhiệt CPU tốt nhất hiện nay. Với thiết kế đẹp mắt, hiệu suất tản nhiệt tốt, Hyper 212 RGB Black Edition là sự lựa chọn lý tưởng cho những ai muốn nâng cấp hệ thống tản nhiệt cho máy tính của mình. Sản phẩm sử dụng quạt tản nhiệt RGB 120mm giúp tản nhiệt hiệu quả và đồng thời tạo điểm nhấn cho hệ thống máy tính." },
                        new Product { ProductName = "Tản nhiệt CPU Cooler Master Hyper 212 EVO", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1440000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tản nhiệt CPU Cooler Master Hyper 212 EVO là một trong những sản phẩm tản nhiệt CPU tốt nhất hiện nay. Với thiết kế đẹp mắt, hiệu suất tản nhiệt tốt, Hyper 212 EVO là sự lựa chọn lý tưởng cho những ai muốn nâng cấp hệ thống tản nhiệt cho máy tính của mình. Sản phẩm sử dụng quạt tản nhiệt 120mm giúp tản nhiệt hiệu quả và đồng thời tạo điểm nhấn cho hệ thống máy tính." },

                        new Product { ProductName = "FAN NZXT Aer RGB 2 Series 120mm Triple Starter (HF-2812C-T1)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1960000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "FAN NZXT Aer RGB 2 Series 120mm Triple Starter (HF-2812C-T1) tích hợp hoàn toàn vào hệ sinh thái HUE 2. Quạt Aer RGB 2 mang đến luồng không khí tối ưu và các tùy chỉnh ánh sáng tiên tiến. Kết nối tối đa năm quạt cho mỗi kênh hoặc kết hợp với bất kỳ phụ kiện HUE 2 nào. Trong bất kỳ kênh nào và đồng bộ hóa chúng bằng CAM để có hiệu ứng ánh sáng tuyệt vời hoạt động hài hòa hoàn hảo. Với ổ trục động chất lỏng và đầu cánh nhỏ. Quạt Aer RGB 2 cung cấp khả năng làm mát nâng cao đồng thời giảm thiểu tiếng ồn." },
                        new Product { ProductName = "Tản nhiệt khí Noctua NH-U12DX I4", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1890000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Dòng máy làm mát DX của Noctua đã trở thành sự lựa chọn mặc định. Trong các giải pháp làm mát êm hiệu suất cao cho CPU Intel Xeon. Phiên bản i4 mới nhất hỗ trợ các nền tảng Xeon dựa trên LGA2011 (cả Square ILM và Narrow ILM ), LGA1356 và LGA1366 và mẫu 12cm.\r\n\r\nNH-U12DX i4 được trang bị quạt NF-F12 Focused Flow của Noctua với PWM để kiểm soát tốc độ tự động. Do thiết kế mỏng, NH-U12DX i4 giờ đây cung cấp khả năng truy cập dễ dàng vào các khe cắm bộ nhớ. Và khả năng tương thích tốt hơn với các mô-đun cao." },
                        new Product { ProductName = "Tản nhiệt khí Noctua NH-L9X65", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1720000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tản nhiệt khí Noctua NH-L9X65 là phiên bản cao hơn, nâng cao hiệu suất của máy làm mát cấu hình thấp NH-L9 của Noctua . Với chiều cao 65 thay vì 37mm và bốn ống dẫn nhiệt thay vì hai ống dẫn nhiệt. NH-L9x65 mạnh mẽ hơn nhưng vẫn giữ được kích thước 95x95mm. Đảm bảo 100% RAM và khả năng tương thích PCIe trên các bo mạch chủ ITX của Intel và làm cho máy làm mát thuận tiện hơn.\r\n\r\nNH-L9x65 chạy rất êm nhờ quạt cao cấp NF-A9x14 được tối ưu hóa cao. Hỗ trợ điều khiển tốc độ hoàn toàn tự động thông qua PWM. Kết hợp với hệ thống gắn đa ổ cắm SecuFirm2 ™ chuyên nghiệp dành cho Intel và AMD." },
                        new Product { ProductName = "Fan Case Noctua NF-A12X15 PWM Chromax Black Swap", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 770000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nhận được hàng chục khuyến nghị từ báo chí quốc tế. NF-A12x15 của Noctua đã trở thành sự lựa chọn cao cấp. Được kiểm chứng cho nhu cầu làm mát mỏng 120mm. Hiệu quả nổi tiếng của nó đã thuyết phục hàng chục nghìn khách hàng trên khắp thế giới." },
                        new Product { ProductName = "Tản nhiệt khí Cooler Master MasterAir MA624 Stealth – MAM-D6PS-314PK-R1", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2770000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nắp trên bằng nhôm đen ẩn giấu phù hợp với phần cứng toàn màu đen trong suốt để mang lại một lớp hoàn thiện cao cấp. Cooler Master thiết kế phù hợp với những khách hàng thích sự tinh tế, đơn giản nhưng vẫn mang lại sự sang trọng. Thiết kế tối ưu cho khả năng tương thích của card đồ họa trên nền tảng ATX và M-ATX." },
                        new Product { ProductName = "Tản Nhiệt Khí Cooler Master Hyper 212 LED – RR-212L-16PR-R1", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 760000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tản Nhiệt Khí Cooler Master Hyper 212 LED – RR-212L-16PR-R1 với bốn ống dẫn nhiệt tiếp xúc trực tiếp. Mang lại hiệu quả cao hơn và tản nhiệt tuyệt vời so với các đế kim loại thông thường. Quạt PWM 120mm mới với miếng cao su chống rung và vòng quay từ 600-2000 RPM cùng nắp chụp nhanh để lắp đặt quạt dễ dàng." },
                        new Product { ProductName = "Quạt Case Corsair FAN HD 140 RGB LED – Hộp 2 FAN – with controller (CO-9050069-WW)", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2090000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Quạt Case Corsair FAN HD 140 RGB LED – Hộp 2 FAN – with controller (CO-9050069-WW) kết hợp 12 đèn LED RGB. Được gắn trên khung độc lập và khả năng phân phối khí tuyệt vời. Mang lại yếu tố tuyệt đẹp cho thiết bị của bạn. Đặt quạt của bạn làm choáng với chế độ tắt dần, thở, nhấp nháy và các chế độ đèn LED khác. Quạt HD140 RGB LED được thiết kế riêng. Cho những người muốn làm cho giàn của họ vượt trội hơn tất cả những người khác." },
                        new Product { ProductName = "Quạt tản nhiệt Thermaltake TOUGHAIR 310", CategoryID = 1, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1050000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Quạt tản nhiệt Thermaltake TOUGHAIR 310 là một giải pháp làm mát CPU dạng tháp đơn (single tower) được thiết kế để mang lại hiệu suất vượt trội trong phân khúc tầm trung. Sản phẩm này được trang bị 4 ống dẫn nhiệt (heat pipes) bằng đồng đường kính 6mm, áp dụng thiết kế hình chữ U giúp tăng cường lưu thông nhiệt và cải thiện khả năng tản nhiệt, hỗ trợ CPU có TDP lên đến 170W. Khối tản nhiệt làm từ nhôm với cấu trúc vây bất đối xứng (asymmetric fin structure), cho phép luồng không khí đi qua dễ dàng hơn, giảm thiểu rối loạn khí và nâng cao hiệu quả làm mát." },
                // phụ kiện
                // Balo_túi chống sốc 71 - 80
                        new Product { ProductName = "Balo Laptop Targus TSB883 Safire Business Casual Backpack 15.6 inch", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 790000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Balo Laptop Targus TSB883 Safire Business Casual Backpack 15.6 inch là một sản phẩm thuộc dòng Safire của thương hiệu Targus đến từ Mỹ, nổi tiếng với các thiết kế tinh tế và chất lượng cao. Được thiết kế dành cho người dùng hiện đại, balo này kết hợp phong cách tối giản với tính năng bảo vệ tối ưu, phù hợp cho công việc, học tập hoặc di chuyển hàng ngày." },
                        new Product { ProductName = "Túi xách Laptop TARGUS TSS898-70 Business Casual Slipcase cho Laptop 15.6 inch", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 749000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Chất liệu chính của Targus TSS898-70 là polyester cao cấp, chống thấm nước, chống rách và phai màu, đảm bảo độ bền cao và bảo vệ đồ đạc bên trong an toàn. Phần tay cầm mềm mại và khóa kéo chắc chắn tạo điểm nhấn tinh tế, vừa thời trang vừa thực dụng. Với màu đen thanh lịch, sản phẩm phù hợp cho cả nam và nữ, dễ dàng phối hợp trong nhiều hoàn cảnh." },
                        new Product { ProductName = "Lenovo Legion 15.6 inch Recon Gaming Backpack", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 800000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Balo có ngăn đựng laptop chuyên dụng, được lót đệm cẩn thận, hỗ trợ các dòng máy lên đến 15.6 inch (kích thước tối đa khoảng 267 x 30 x 362 mm), đảm bảo an toàn cho thiết bị trong quá trình vận chuyển. Ngăn chính rộng rãi với nhiều túi nhỏ được bố trí hợp lý, cho phép người dùng sắp xếp phụ kiện chơi game như chuột, tai nghe, bàn phím hoặc các vật dụng cá nhân khác một cách ngăn nắp. Ngoài ra, balo còn có ngăn phụ phía trước với các khe cắm bút và vòng treo kính, cùng túi đựng chai nước ở hai bên, tăng tính tiện lợi." },
                        new Product { ProductName = "Balo Laptop 15.6 inch Mikkor The Jack Backpack", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 940000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Balo được làm từ chất liệu vải Polyester 900D Twill kết hợp lớp phủ PU và WR (water-repellent), mang lại khả năng chống thấm nước hiệu quả, bảo vệ đồ đạc bên trong khỏi thời tiết ẩm ướt. Kích thước của balo là 46 x 30 x 17 cm, trọng lượng khoảng 900g, đủ rộng rãi để chứa laptop 15.6 inch trong ngăn chống sốc chuyên dụng, cùng với sách vở, tài liệu hoặc quần áo. Tải trọng tối đa lên đến 20kg, đảm bảo độ bền và chắc chắn khi sử dụng." },
                        new Product { ProductName = "Balo Laptop 15.6 inch SimpleCarry K4", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 840000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Balo Laptop 15.6 inch SimpleCarry K4 là một sản phẩm nổi bật từ thương hiệu SimpleCarry, được thiết kế dành cho những ai cần một chiếc balo tiện dụng, bền bỉ và phong cách. Với sự kết hợp giữa tính thực dụng và thẩm mỹ, balo này phù hợp cho học sinh, sinh viên, nhân viên văn phòng hoặc những chuyến đi ngắn ngày." },
                        new Product { ProductName = "Túi chống sốc laptop 13.3 inch (Đen)", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 80000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Túi thường được làm từ chất liệu polyester hoặc nylon cao cấp, có khả năng chống thấm nước nhẹ, giúp bảo vệ laptop khỏi mưa nhỏ và bụi bẩn. Bên trong, lớp đệm chống sốc (thường là mút PE, cao su non hoặc nhung mềm) được thiết kế dày dặn, ôm sát máy, giảm thiểu nguy cơ hư hỏng do va đập hoặc rơi rớt. Một số mẫu túi còn tích hợp công nghệ bảo vệ góc (CornerArmor) hoặc lớp lót chống trầy xước, tăng cường an toàn cho laptop." },
                        new Product { ProductName = "Túi chống sốc Laptop Mácbook Ultrabook 13.3 inch 14 inch 15 inch 15.6 inch 16 inch", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 49000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bạn đang tìm kiếm một chiếc túi chống sốc chất lượng để bảo vệ Laptop, MacBook hay Ultrabook của mình? Túi chống sốc cao cấp dành cho các dòng máy 13.3 inch, 14 inch, 15 inch, 15.6 inch và 16 inch chính là lựa chọn lý tưởng!" },
                        new Product { ProductName = "Túi chống sốc Laptop 15.6 inch Togo", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 900000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Túi chống sốc Laptop 15.6 inch Togo là lựa chọn hoàn hảo để bảo vệ laptop của bạn khỏi va đập, trầy xước và ảnh hưởng từ môi trường bên ngoài. Với thiết kế mỏng nhẹ, tinh tế, lớp vải cao cấp chống nước và lớp đệm êm ái bên trong, sản phẩm giúp hạn chế sốc và bảo vệ thiết bị an toàn trong quá trình di chuyển. Túi trang bị khóa kéo chắc chắn, đường may tỉ mỉ, dễ dàng mang theo trong balo hoặc cầm tay. Đây là phụ kiện lý tưởng cho sinh viên, nhân viên văn phòng và những ai thường xuyên di chuyển cùng laptop!" },
                        new Product { ProductName = "Túi Chống Sốc Laptop 13 inch Jinya Office Sleeve", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 612000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Túi chống sốc Laptop 13 inch Jinya Office Sleeve là lựa chọn hoàn hảo để bảo vệ laptop của bạn với thiết kế thanh lịch, mỏng nhẹ và chất liệu polyester cao cấp. Túi có lớp lót mềm mại, chống sốc 360 độ, giúp bảo vệ laptop khỏi va đập, trầy xước trong quá trình di chuyển. Đặc biệt, khả năng chống nước, chống bụi bẩn giúp giữ thiết bị luôn an toàn trước mọi điều kiện thời tiết. Ngoài ra, túi còn được trang bị ngăn phụ tiện lợi để đựng phụ kiện như sạc, điện thoại, tai nghe. Đây là phụ kiện lý tưởng cho dân văn phòng, sinh viên và những ai yêu thích phong cách tối giản!" },
                        new Product { ProductName = "Túi chống sốc Laptop 14 inch Jinya Work", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 467000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Túi chống sốc Laptop 14 inch Jinya Work là phụ kiện bảo vệ laptop cao cấp, thiết kế tối giản nhưng hiện đại, phù hợp với dân văn phòng và học sinh, sinh viên. Được làm từ chất liệu vải polyester chống nước, kết hợp với lớp lót êm ái bên trong, túi giúp bảo vệ laptop khỏi trầy xước, va đập và bụi bẩn. Thiết kế mỏng nhẹ, có ngăn phụ tiện lợi để đựng phụ kiện như sạc, chuột, tai nghe. Với khóa kéo bền bỉ và kiểu dáng thanh lịch, đây là lựa chọn lý tưởng để mang theo laptop khi đi làm, đi học hoặc công tác. " },
                // Cáp sạc - bộ sạc 81 - 90
                        new Product { ProductName = "Cáp sạc nhanh USB Type-C Baseus Cafule Series 2A 1m", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 99000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Cáp sạc nhanh USB Type-C Baseus Cafule Series 2A 1m là một sản phẩm chất lượng từ thương hiệu Baseus, nổi tiếng với các phụ kiện công nghệ cao cấp. Sản phẩm được thiết kế với chất liệu dây cáp chất lượng cao, bền bỉ, chống rối và chống đứt, giúp sạc nhanh và an toàn cho thiết bị của bạn. Cáp hỗ trợ sạc nhanh 2A, tương thích với nhiều thiết bị sử dụng cổng USB Type-C, phù hợp cho việc sạc nhanh điện thoại, máy tính bảng, laptop, tai nghe, loa, camera, máy ảnh, máy quay, máy chơi game và nhiều thiết bị khác." },
                        new Product { ProductName = "Dây cáp sạc nhanh USB Type C to IOS", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 80000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Dây cáp sạc nhanh USB Type C to iOS / USB to iOS / Type C to Type C 1m 20W là phụ kiện tiện lợi, hỗ trợ sạc nhanh lên đến 20W và truyền dữ liệu tốc độ cao. Với thiết kế bền bỉ, lõi đồng cao cấp, cáp đảm bảo hiệu suất ổn định, an toàn cho thiết bị. Tương thích với nhiều thiết bị Apple (iPhone, iPad), điện thoại Android, MacBook và các thiết bị có cổng USB-C. Dây dài 1m giúp dễ dàng sử dụng trong nhiều tình huống, từ làm việc đến di chuyển. Lựa chọn lý tưởng cho người dùng cần một sợi cáp sạc nhanh, bền và đa năng!" },
                        new Product { ProductName = "Sạc Máy Tính Laptop Asus X550 X550E X550LN X550LD X550L X550LC", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 349000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc máy tính laptop Asus X550/X550E/X550LN/X550LD/X550L/X550LC là bộ sạc chính hãng, được thiết kế để cung cấp nguồn điện ổn định và an toàn cho các dòng laptop Asus X550 series. Với điện áp đầu ra 19V - 3.42A và công suất 65W, sạc đảm bảo hiệu suất hoạt động tốt, giúp laptop hoạt động mượt mà mà không bị gián đoạn. Sản phẩm được trang bị chế độ bảo vệ quá dòng, quá áp và chống đoản mạch, giúp tăng tuổi thọ pin và bảo vệ thiết bị tối ưu. Thiết kế nhỏ gọn, bền bỉ, phù hợp cho học tập, làm việc và di chuyển. Đây là lựa chọn lý tưởng cho người dùng laptop Asus X550 cần thay thế hoặc dự phòng sạc chất lượng!" },
                        new Product { ProductName = "Sạc Laptop HP 18.5V-3.5A (60-65W)", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 249000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop HP 18.5V-3.5A (60-65W) là bộ sạc chính hãng, được thiết kế để cung cấp nguồn điện ổn định và an toàn cho các dòng laptop HP. Với công suất 60-65W, điện áp đầu ra 18.5V - 3.5A, sản phẩm đảm bảo hiệu suất sạc nhanh chóng, giúp laptop hoạt động bền bỉ. Sạc được trang bị chuẩn đầu cắm 7.4mm x 5.0mm, tương thích với nhiều mẫu laptop HP phổ biến. Ngoài ra, thiết kế nhỏ gọn, chất liệu cao cấp và cơ chế bảo vệ quá áp, quá tải giúp tăng tuổi thọ và an toàn khi sử dụng. Đây là lựa chọn lý tưởng để thay thế hoặc dự phòng cho laptop HP của bạn! " },
                        new Product { ProductName = "Sạc Laptop Acer 19.5V - 3.42A chân thường 5.5*1.7mm ", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 249000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop Acer 19.5V - 3.42A chân 5.5*1.7mm là phụ kiện thay thế hoặc dự phòng lý tưởng cho các dòng laptop Acer. Với điện áp 19.5V và dòng ra 3.42A, sạc cung cấp nguồn điện ổn định, giúp thiết bị hoạt động bền bỉ và an toàn. Thiết kế chân cắm 5.5*1.7mm phổ biến, tương thích với nhiều mẫu laptop Acer như Aspire, TravelMate, Extensa,… Sản phẩm được trang bị mạch bảo vệ quá dòng, quá áp, đảm bảo an toàn trong quá trình sử dụng. Đây là lựa chọn đáng tin cậy giúp bạn duy trì hiệu suất làm việc mà không lo gián đoạn!" },
                        new Product { ProductName = "Sạc Laptop LENOVO 90W - 20V - 4.5A CHÂN USB - TTA - LE90", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 179000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop LENOVO 90W - 20V - 4.5A CHÂN USB - TTA - LE90 là giải pháp sạc điện lý tưởng dành cho các dòng máy tính xách tay Lenovo. Được thiết kế với công suất 90W và điện áp đầu ra ổn định 20V-4.5A, sản phẩm đảm bảo cung cấp nguồn điện an toàn và hiệu quả cho thiết bị của bạn.\r\nVới cổng kết nối USB hiện đại, sạc laptop này mang đến sự tiện lợi và dễ dàng trong quá trình sử dụng. Sản phẩm được sản xuất theo tiêu chuẩn chất lượng cao, đảm bảo độ bền và hiệu suất sạc tối ưu. Thương hiệu TTA - LE90 cam kết mang đến trải nghiệm sử dụng tuyệt vời với khả năng tương thích hoàn hảo cho các dòng laptop Lenovo." },
                        new Product { ProductName = "Bộ Sạc Laptop Và Các Thiết Bị Công Nghệ 8 Đầu Công Suất 96W", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 185000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ Sạc Laptop Và Các Thiết Bị Công Nghệ 8 Đầu Công Suất 96W là giải pháp sạc đa năng hiện đại dành cho người dùng có nhiều thiết bị công nghệ khác nhau. Với công suất mạnh mẽ 96W, sản phẩm đáp ứng nhu cầu sạc cho hầu hết các dòng laptop và thiết bị điện tử phổ biến trên thị trường hiện nay.\r\nĐặc biệt, bộ sạc được trang bị 8 đầu kết nối chuyên dụng, cho phép tương thích với đa dạng các thiết bị từ nhiều thương hiệu khác nhau như Dell, HP, Lenovo, Asus, Acer, và nhiều hãng khác. Thiết kế đa năng này giúp bạn tiết kiệm không gian và chi phí khi không cần phải mua nhiều bộ sạc riêng biệt." },
                        new Product { ProductName = "Sạc laptop Dell Precision 5530 5540 - Adapter Laptop Dell 19.5V 6.67A 130W Oval", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 850000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop Dell Precision 5530 5540 - Adapter Laptop Dell 19.5V 6.67A 130W Oval là sự lựa chọn hoàn hảo để đảm bảo hiệu suất và tuổi thọ cho chiếc máy tính của bạn. Với công suất lên tới 130W, thiết bị cung cấp nguồn điện ổn định, đáng tin cậy, giúp máy hoạt động mượt mà ngay cả trong những tác vụ nặng. Sạc được thiết kế với chuẩn 19.5V 6.67A, phù hợp hoàn hảo cho các dòng Dell Precision 5530 và 5540. Kiểu dáng Oval hiện đại, nhỏ gọn, dễ dàng mang theo, đặc biệt phù hợp với những người dùng thường xuyên di chuyển." },
                        new Product { ProductName = "Sạc Laptop Asus Zenbook UX425E (20V Vuông 65w, Type C)", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 550000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop Asus Zenbook UX425E (20V Vuông 65W, Type C) là một sản phẩm chất lượng cao, được thiết kế đặc biệt để cung cấp nguồn điện ổn định và an toàn cho dòng laptop Asus Zenbook UX425E. Với công suất 65W và chuẩn sạc 20V, thiết bị này đảm bảo hiệu suất tối ưu cho máy tính của bạn. Đầu cắm Type C hiện đại, tiện lợi, phù hợp với xu hướng công nghệ mới. Thiết kế vuông vắn, nhỏ gọn, dễ dàng mang theo khi di chuyển. Đây là sự lựa chọn lý tưởng để bảo vệ và duy trì hiệu suất của laptop Asus Zenbook UX425E." },
                        new Product { ProductName = "Sạc laptop Dell 20V-4.5A 90W USB Type C Oval", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 690000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Sạc Laptop Dell 20V-4.5A 90W USB Type C Oval là phụ kiện hoàn hảo dành cho các dòng laptop Dell hiện đại. Với công suất 90W và chuẩn sạc 20V-4.5A, thiết bị cung cấp nguồn năng lượng ổn định, đảm bảo hiệu suất hoạt động tối đa cho máy tính của bạn. Đầu sạc USB Type C tiên tiến giúp kết nối nhanh chóng và tiện lợi, phù hợp với các xu hướng công nghệ hiện nay. Thiết kế Oval nhỏ gọn, hiện đại, dễ dàng mang theo trong những chuyến công tác hay du lịch. Đây là sự lựa chọn tối ưu để duy trì và bảo vệ tuổi thọ của laptop Dell." },
                // Giá đỡ laptop 91 - 100
                        new Product { ProductName = "Giá Đỡ Tản Nhiệt Đa Năng Baseus UltraStable Pro Series Xoay 360", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 674000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop 360 độ là một sản phẩm tiện ích, giúp bạn dễ dàng điều chỉnh góc độ và vị trí của laptop một cách linh hoạt. Với thiết kế 360 độ xoay, bạn có thể dễ dàng điều chỉnh góc độ, độ cao và hướng của laptop một cách linh hoạt, phù hợp với nhu cầu làm việc, học tập hoặc giải trí của bạn. Sản phẩm được làm từ chất liệu cao cấp, bền bỉ, chịu lực tốt, đảm bảo an toàn cho laptop của bạn. Thiết kế nhỏ gọn, dễ dàng sử dụng, phù hợp với nhiều môi trường làm việc khác nhau. Đây là phụ kiện không thể thiếu cho những người thường xuyên sử dụng laptop!" },
                        new Product { ProductName = "Giá đỡ laptop SENZANS", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 650000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop SENZANS là giải pháp hoàn hảo để nâng cao trải nghiệm sử dụng laptop của bạn. Với thiết kế nhỏ gọn, hiện đại và chất liệu hợp kim nhôm bền bỉ, sản phẩm không chỉ đảm bảo độ chắc chắn mà còn hỗ trợ tản nhiệt hiệu quả. Đặc biệt, khả năng điều chỉnh độ cao và xoay 360 độ giúp tối ưu hóa tư thế ngồi, bảo vệ sức khỏe người dùng. Đây chắc chắn là phụ kiện hữu ích dành cho dân văn phòng, học sinh, sinh viên và những ai thường xuyên làm việc với laptop." },
                        new Product { ProductName = "SKU312 - Giá đỡ Laptop Xoay 360 độ YL-906 có quạt", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 245000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ Laptop Xoay 360 độ YL-906 có quạt là một sản phẩm đa năng, lý tưởng cho những ai thường xuyên sử dụng laptop. Với thiết kế xoay 360 độ, sản phẩm mang lại sự linh hoạt tối đa, giúp bạn dễ dàng điều chỉnh góc độ phù hợp với tư thế ngồi. Đặc biệt, quạt tản nhiệt tích hợp giúp làm mát laptop hiệu quả, bảo vệ thiết bị khỏi tình trạng quá nhiệt khi sử dụng lâu dài.\r\n\r\nSản phẩm được làm từ chất liệu thép cacbon bền bỉ, phù hợp với các dòng laptop từ 10 đến 17 inch. Đây là lựa chọn hoàn hảo để nâng cao trải nghiệm làm việc và học tập của bạn." },
                        new Product { ProductName = "Giá đỡ kiêm đế tản nhiệt làm mát DIGIMIX", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 690000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop thường được làm bằng nhôm chắc chắn bề bỉ theo thời gian hoặc bằng gỗ hay nhựa giá rẻ nhưng đều có chung chức năng hỗ trợ tản nhiệt, làm mát máy tính laptop, macbook sử dụng trong thời gian dài. Giá đỡ laptop gấp gọn, điều chỉnh độ cao tùy ý đang là xu hướng hiện nay cho văn phòng, công ty." },
                        new Product { ProductName = "Giá đỡ laptop tablet DIGIMIX ", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 489000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop tablet DIGIMIX là phụ kiện thông minh được thiết kế để hỗ trợ tối ưu trong công việc và giải trí. Với chất liệu hợp kim nhôm cao cấp, sản phẩm mang lại độ bền vượt trội cùng vẻ ngoài sang trọng hiện đại. Thiết kế linh hoạt với khả năng điều chỉnh độ cao và góc nghiêng giúp bạn có tư thế ngồi thoải mái, bảo vệ sức khỏe lâu dài." },
                        new Product { ProductName = "Giá đỡ tản nhiệt hợp kim nhôm xoay 360 độ, kèm quạt tản nhiệt làm mát T602, T628, T619 cao cấp SIZIBOX", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 689000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ tản nhiệt hợp kim nhôm xoay 360 độ, kèm quạt tản nhiệt làm mát T602, T628, T619 cao cấp SIZIBOX là một sản phẩm đa năng, được thiết kế để tối ưu hóa trải nghiệm sử dụng laptop. Với chất liệu hợp kim nhôm cao cấp, sản phẩm mang lại độ bền vượt trội và khả năng chịu lực tốt." },
                        new Product { ProductName = "Giá đỡ laptop bằng nhựa hợp kim nhôm, đế kim loại", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 180000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop bằng nhựa hợp kim nhôm, đế kim loại, hỗ trợ nâng cao, làm mát, điều chỉnh, di động là một sản phẩm tiện ích, được thiết kế để mang lại sự thoải mái và hiệu quả tối ưu khi sử dụng laptop. Với sự kết hợp giữa nhựa cao cấp và hợp kim nhôm, sản phẩm không chỉ bền bỉ mà còn nhẹ nhàng, dễ dàng mang theo." },
                        new Product { ProductName = "Giá đỡ laptop MacBook", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 550000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Giá đỡ laptop MacBook là phụ kiện lý tưởng để nâng cao trải nghiệm sử dụng thiết bị của bạn. Với thiết kế hiện đại, chất liệu cao cấp như hợp kim nhôm, sản phẩm không chỉ mang lại sự chắc chắn mà còn giúp tản nhiệt hiệu quả, bảo vệ máy khỏi tình trạng quá nhiệt." },
                        new Product { ProductName = "Giá Đỡ kim loại cho laptop máy tính bảng hỗ trợ tản nhiệt ", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 150000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Nâng tầm trải nghiệm làm việc và giải trí với giá đỡ kim loại cao cấp dành cho laptop và máy tính bảng! Sản phẩm được thiết kế chắc chắn, bền bỉ, giúp nâng thiết bị lên góc độ tối ưu, mang lại tư thế ngồi thoải mái, hạn chế đau vai gáy khi sử dụng lâu dài." },
                        new Product { ProductName = "Giá đỡ Laptop, máy tính bảng gấp gọn Orico PFB-A24", CategoryID = 2, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 198000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Orico PFB-A24 được thiết kế từ hợp kim nhôm cao cấp, chắc chắn và bền bỉ. Với khả năng gấp gọn tiện lợi, dễ dàng mang theo mọi nơi. Góc nghiêng tối ưu giúp cải thiện tư thế ngồi, giảm mỏi cổ và hỗ trợ tản nhiệt hiệu quả, giữ cho thiết bị luôn mát mẻ. Phù hợp với nhiều dòng laptop, máy tính bảng. Lựa chọn hoàn hảo cho công việc và giải trí!" },
                // Thiết bị mạng
                // Bộ mở rộng sóng wifi 101 - 110
                        new Product { ProductName = "Bộ Mở Rộng Sóng Wifi Xiaomi Mi Wifi Range Extender Pro", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 289000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ mở rộng sóng Wifi Xiaomi Mi Wifi Range Extender Pro là giải pháp tối ưu để mở rộng vùng phủ sóng Wifi trong nhà, giúp bạn truy cập Internet một cách nhanh chóng và ổn định hơn. Với thiết kế nhỏ gọn, dễ dàng lắp đặt và kết nối, sản phẩm giúp bạn mở rộng vùng phủ sóng Wifi đến mọi góc phòng, giảm tình trạng mất sóng, giật lag khi sử dụng Internet. Đặc biệt, bộ mở rộng sóng Wifi Xiaomi Mi Wifi Range Extender Pro hỗ trợ tốc độ truyền tải lên đến 300Mbps, tương thích với nhiều thiết bị và chuẩn Wifi phổ biến hiện nay. Đây là lựa chọn hoàn hảo để nâng cao trải nghiệm sử dụng Internet của bạn!" },
                        new Product { ProductName = "Bộ Mở Rộng Sóng Wifi TP-Link TL-WA850RE", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 399000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ mở rộng sóng Wifi TP-Link TL-WA850RE là giải pháp tối ưu để mở rộng vùng phủ sóng Wifi trong nhà, giúp bạn truy cập Internet một cách nhanh chóng và ổn định hơn mà không cần kéo dây mạng. Với thiết kế nhỏ gọn, dễ dàng lắp đặt và kết nối, sản phẩm giúp bạn mở rộng vùng phủ sóng Wifi đến mọi góc phòng, giảm tình trạng mất sóng, giật lag khi sử dụng Internet. Đặc biệt, bộ mở rộng sóng Wifi TP-Link TL-WA850RE hỗ trợ tốc độ truyền tải lên đến 300Mbps, tương thích với nhiều thiết bị và chuẩn Wifi phổ biến hiện nay. Đây là lựa chọn hoàn hảo để nâng cao trải nghiệm sử dụng Internet của bạn!" },
                        new Product { ProductName = "Thiết bị mở rộng sóng Wifi Tp-link băng tần kép AC1200 RE315", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 469000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "TP-Link RE315 giúp mở rộng phạm vi WiFi, tăng cường tín hiệu mạnh mẽ với băng tần kép AC1200 (2.4GHz & 5GHz), đảm bảo tốc độ ổn định cho mọi thiết bị. Thiết kế cắm trực tiếp vào ổ điện, dễ dàng cài đặt và sử dụng. Hỗ trợ chế độ Access Point, mang lại trải nghiệm mạng nhanh và liền mạch cho gia đình, văn phòng!" },
                        new Product { ProductName = "Bộ kích sóng Wifi 6 TP-Link RE505X AX1500", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 849000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Re505X được trang bị công nghệ không dây wifi 6 mới nhất, tốc độ nhanh cùng dung lượng cực lớn, độ trễ giảm tối thiểu nhất. Công nghệ này còn giúp giảm tắc nghẽn mạng hiệu quả cho wifi nhà bạn. RE505X được trang bị đèn tín hiệu thông minh, có khả năng xác định vị trí để phủ sóng wifi tốt nhất cho toàn khu vực. Thiết bị còn giúp chỉ ra cường độ tín hiệu tại vị trí bạn đang đứng." },
                        new Product { ProductName = "Bộ kích sóng Wifi Totolink băng tần kép EX1200T băng tần kép AC1200", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 499000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ mở rộng sóng Wi-Fi băng tần kép AC1200 TOTOLINK EX1200T sẽ là lựa chọn phù hợp cho người dùng đang có nhu cầu tìm kiếm một thiết bị mở rộng phạm vi phát Wi-Fi trong ngôi nhà của mình, nhưng vẫn đảm bảo tính linh hoạt, cài đặt dễ dàng tiện lợi." },
                        new Product { ProductName = "Bộ kích sóng Wifi TP-Link Re205 Ac750", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 439000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bộ kích sóng Wifi TP-Link Re205 Ac750 đến từ thương hiệu nổi tiếng về các sản phẩm có chất lượng TP-Link. Bộ kích sóng này có thiết kế cực kì nhỏ gọn là 52 x 34 x 65mm cho người dùng dễ dàng lắp đặt trong căn phòng của mình mà không sợ chiếm quá nhiều diện tích. Các chi tiết được hoàn thiện tỉ mỉ từ các góc cạnh cho tới các cổng kết nối giúp sản phẩm thêm phần cứng cáp và người dùng dễ dàng sử dụng hơn." },
                        new Product { ProductName = "Thiết bị mở rộng sóng Wifi băng tần kép chuẩn AC1200 Totolink EX1200T", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 569000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Thiết bị mở rộng sóng WiFi băng tần kép chuẩn AC1200 Totolink EX1200T sử dụng thiết kế công thái học nhỏ gọn với trang bị chuôi cắm, hỗ trợ cắm tường tiết kiệm không gian. Vỏ ngoài chắc chắn sẽ giúp bạn có thể lắp đặt ở cả những vị trí thấp mà không lo trầy xước hay nứt vỡ khi va đập." },
                        new Product { ProductName = "Bộ kích sóng Wifi Totolink băng tần kép EX1200T băng tần kép AC1200", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 499000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tối ưu trong thiết kế và lắp đặt linh kiện nên bộ mở rộng sóng Wi-Fi băng tần kép AC1200 TOTOLINK EX1200T có kích thước vô cùng nhỏ gọn, trọng lượng siêu nhẹ. Giúp bạn có thể mang theo thiết bị để kết nối trên mọi chuyến hành trình." },
                        new Product { ProductName = "Thiết bị mở rộng sóng Wifi Mercusys ME10 300Mbps", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 185000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Thiết bị mở rộng sóng WiFi Mercusys ME10 300 Mbps được tăng cường tín hiệu WiFi với tốc độ tối đa 300Mbps ở băng tần 2.4GHz cùng cổng 10/100Mbps nhanh. Thêm vào đó, mẫu bộ kích sóng wifi này còn hỗ trợ mở rộng phạm vi phủ sóng với khả năng thiết lập dễ dàng. Ngoài ra, đèn LED nhiều màu báo vị trí phù hợp giúp người dùng tìm được điểm đặt hoàn hảo trong không gian." },
                        new Product { ProductName = "Bộ kích sóng WiFi Tenda A9 chuẩn N tốc độ 300Mbps", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 219000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Thiết bị mở rộng sóng Wi-Fi chuẩn N tốc độ 300Mbps Tenda A9 được thiết kế để tăng cường vùng phủ sóng rộng hơn với tốc độ ổn định. Thiết bị được sử dụng đơn giản, nên hứa hẹn sẽ là lựa chọn để loại bỏ điểm chết WiFi cho gia đình. Thiết bị mở rộng sóng Wi-Fi chuẩn N tốc độ 300Mbps Tenda A9 sở hữu thiết kế gọn gàng chỉ 111 x 56 x 47.7mm. Do đó dù đặt nó ở đâu cũng không lo tốn quá nhiều không gian của ổ cắm điện." },
                // Router wifi 111 - 120
                        new Product { ProductName = "Router Wifi Tenda AC6 AC1200", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 599000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Router Wifi Tenda AC6 AC1200 là một trong những sản phẩm router wifi tốt nhất hiện nay, với tốc độ truyền tải lên đến 1200Mbps, giúp bạn truy cập Internet một cách nhanh chóng và ổn định hơn. Với thiết kế nhỏ gọn, dễ dàng lắp đặt và kết nối, sản phẩm giúp bạn mở rộng vùng phủ sóng Wifi đến mọi góc phòng, giảm tình trạng mất sóng, giật lag khi sử dụng Internet. Đặc biệt, router wifi Tenda AC6 AC1200 hỗ trợ tốc độ truyền tải lên đến 1200Mbps, tương thích với nhiều thiết bị và chuẩn Wifi phổ biến hiện nay. Đây là lựa chọn hoàn hảo để nâng cao trải nghiệm sử dụng Internet của bạn!" },
                        new Product { ProductName = "Router Wifi Tenda AC10 AC1200", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 699000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Router Wifi Tenda AC10 AC1200 là một trong những sản phẩm router wifi tốt nhất hiện nay, với tốc độ truyền tải lên đến 1200Mbps, giúp bạn truy cập Internet một cách nhanh chóng và ổn định hơn. Với thiết kế nhỏ gọn, dễ dàng lắp đặt và kết nối, sản phẩm giúp bạn mở rộng vùng phủ sóng Wifi đến mọi góc phòng, giảm tình trạng mất sóng, giật lag khi sử dụng Internet. Đặc biệt, router wifi Tenda AC10 AC1200 hỗ trợ tốc độ truyền tải lên đến 1200Mbps, tương thích với nhiều thiết bị và chuẩn Wifi phổ biến hiện nay. Đây là lựa chọn hoàn hảo để nâng cao trải nghiệm sử dụng Internet của bạn!" },

                        new Product { ProductName = "Tenda 4G03 - Router WiFi N300", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1280000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Với khả năng thu sóng di động 4G LTE , Tenda 4G03 có thể cung cấp Internet cho bạn ở bất cứ nơi đâu mà không cần triển khai cáp quang.Thích hợp cho những khu vực như ngoại ô, nông thôn, trang trại, gia đình đi du lịch. Tenda 4G03 cung cấp tốc độ tối đa của LTE CAT4 đến 150Mbps , bạn có thể tận hưởng phim HD mượt mà, download file dung lượng lớn và video call ở bất cứ đâu." },
                        new Product { ProductName = "Tenda AC10 V4.0 - Router WiFi băng tần kép AC1200 Gigabit ", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 850000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Với CPU tốc độ 1GHz tiến trình 28nm, Tenda AC10 đảm bảo khả năng xử lý mạnh mẽ và tiết kiệm năng lượng, ít tỏa nhiệt. Đáp ứng tốt với cáp quang với băng thông trải dài từ 50 M, 100 M, 200 M đến thậm chí 1000 M, tận hưởng chơi game và xem video HD mượt mà." },
                        new Product { ProductName = "Tenda F6 - Router WiFi chuẩn N 300Mbps ", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 379000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tenda F6 là router WiFi chuẩn N 300Mbps, phù hợp cho gia đình và văn phòng nhỏ. Với 4 ăng-ten hiệu suất cao, thiết bị đảm bảo tín hiệu mạnh, phủ sóng rộng và kết nối ổn định. Hỗ trợ chế độ Repeater, giúp mở rộng mạng WiFi dễ dàng. Cài đặt đơn giản, giao diện thân thiện – giải pháp tối ưu cho nhu cầu internet hàng ngày! " },
                        new Product { ProductName = "Tenda N301 - Router WiFi chuẩn N tốc độ 300Mbps", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 280000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tenda N301 là router WiFi chuẩn N tốc độ 300Mbps, đáp ứng tốt nhu cầu lướt web, xem phim, học tập và làm việc online. Với 2 ăng-ten mạnh mẽ, thiết bị mang đến kết nối ổn định, phủ sóng rộng cho không gian gia đình và văn phòng nhỏ. Hỗ trợ bảo mật WPA/WPA2, đảm bảo an toàn mạng. Cài đặt đơn giản chỉ trong vài phút, dễ dàng sử dụng ngay!" },
                        new Product { ProductName = "Tenda TX2 Pro - Router Wi-Fi 6 băng tần kép cổng Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1189000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "TX2 Pro là thiết bị Router Wi-Fi 6 với 3 cổng gigabit được thiết kế cho dùng trong gia đình với tốc độ lên đến 1501Mbps (2.4GHz: 300Mbps, 5GHz: 1201Mbps). TX2 Pro được trang bị FEMs hiệu suất cao và 5 ăng-ten ngoài độ lợi cao 6dBi. Hỗ trợ kết nối internet cùng lúc với nhiều thiết bị hơn, độ trễ thấp hơn và hiệu quả truyền tải được cải thiện đáng kể nhờ công nghệ OFDMA + MU-MIMO. Kết nối nhiều thiết bị có dây hơn cho tốc độ truyền tải nhanh hơn với cổng ethernet gigabit, đảm bảo mọi loại thiết bị có dây hoạt động ổn định, mượt mà, giúp bạn tận hưởng mạng với tốc độ cực cao." },
                        new Product { ProductName = "Tenda AC5 - Router WiFi 2 băng tần AC1200", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 519000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Được trang bị công nghệ 802.11ac Wave 2.0 mới, Tenda AC5 cung cấp tốc độ WiFi lên đến 1167 Mbps, truyền dữ liệu nhanh hơn chuẩn 802.11n. Băng tần 5 GHz có thể cung cấp nhiều kênh truyền hơn, giảm nhiễu hơn và mang lại kết nối WiFi ổn định hơn. Ứng dụng công nghệ MU-MIMO cải thiện số lượng thiết bị kết nối đồng thời với mạng WiFi." },
                        new Product { ProductName = "Tenda AC10 V4.0 - Router WiFi băng tần kép AC1200 Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 850000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Với CPU tốc độ 1GHz tiến trình 28nm, Tenda AC10 đảm bảo khả năng xử lý mạnh mẽ và tiết kiệm năng lượng, ít tỏa nhiệt. Đáp ứng tốt với cáp quang với băng thông trải dài từ 50 M, 100 M, 200 M đến thậm chí 1000 M, tận hưởng chơi game và xem video HD mượt mà." },
                        new Product { ProductName = "Tenda 4G05 Router Wifi N300 dùng Sim 4G LTE", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 999000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tenda 4G05 là Router LTE kết hợp LTE và truy cập Ethernet tốc độ cao, tốc độ WiFi lên tới 300Mbps. Tương thích với hầu hết thẻ Sim của các nhà mạng trên thế giới, 4G05 mang lại trải nghiệm Cắm và Dùng đơn giản mà không cần cấu hình . Sản phẩm 4G05 sử dụng cho biệt thự, phòng hội nghị, hệ thống Camera giám sát, cửa hàng lưu động hay mất điện, đảm bảo trải nghiệm mạng ổn định và tốc độ cao." },
                // Switch mạng 120 - 130
                        new Product { ProductName = "Switch mạng Tenda SG105 5 cổng Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 299000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Switch mạng Tenda SG105 5 cổng Gigabit là giải pháp lý tưởng để mở rộng mạng LAN tại nhà hoặc văn phòng." },

                        new Product { ProductName = "Switch mạng Tenda SG108 8 cổng Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 399000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Switch mạng Tenda SG108 8 cổng Gigabit giúp kết nối nhanh chóng, ổn định giữa các thiết bị mạng." },

                        new Product { ProductName = "Switch mạng Tenda S105 5 cổng 10/100Mbps", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 199000m, UnitsInStock = 120,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Switch mạng Tenda S105 với 5 cổng 10/100Mbps, thiết kế nhỏ gọn, tiết kiệm điện năng." },

                        new Product { ProductName = "Switch mạng Tenda S108 8 cổng 10/100Mbps", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 259000m, UnitsInStock = 90,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Switch mạng Tenda S108 với 8 cổng 10/100Mbps, giúp mở rộng mạng LAN dễ dàng." },

                        new Product { ProductName = "Switch mạng TP-Link TL-SG1005D 5 cổng Gigabit", CategoryID = 3, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 319000m, UnitsInStock = 80,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "TP-Link TL-SG1005D là switch 5 cổng Gigabit, hỗ trợ kết nối tốc độ cao, tiết kiệm điện năng." },

                        new Product { ProductName = "Switch mạng TP-Link TL-SG1008D 8 cổng Gigabit", CategoryID = 3, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 449000m, UnitsInStock = 70,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "TP-Link TL-SG1008D với 8 cổng Gigabit giúp mở rộng mạng LAN dễ dàng, hỗ trợ Plug and Play." },

                        new Product { ProductName = "Switch mạng TP-Link TL-SF1005D 5 cổng 10/100Mbps", CategoryID = 3, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 159000m, UnitsInStock = 110,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Switch mạng TP-Link TL-SF1005D với 5 cổng tốc độ 10/100Mbps, thiết kế nhỏ gọn, dễ sử dụng." },

                        new Product { ProductName = "Switch mạng TP-Link TL-SF1008D 8 cổng 10/100Mbps", CategoryID = 3, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 219000m, UnitsInStock = 100,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Switch mạng TP-Link TL-SF1008D với 8 cổng 10/100Mbps, thích hợp cho mạng gia đình và văn phòng nhỏ." },

                        new Product { ProductName = "Switch mạng D-Link DGS-1005A 5 cổng Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 329000m, UnitsInStock = 60,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "D-Link DGS-1005A là switch 5 cổng Gigabit, hỗ trợ kết nối tốc độ cao với công nghệ tiết kiệm điện năng." },

                        new Product { ProductName = "Switch mạng D-Link DGS-1008A 8 cổng Gigabit", CategoryID = 3, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 479000m, UnitsInStock = 50,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "D-Link DGS-1008A với 8 cổng Gigabit, phù hợp cho gia đình và doanh nghiệp nhỏ, hỗ trợ kết nối ổn định." },
                // thiết bị ngoại vi
                // Bàn phím 130 - 140
                        new Product { ProductName = "Bàn phím cơ Logitech G Pro X", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 2490000m, UnitsInStock = 50,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Bàn phím cơ Logitech G Pro X với công tắc có thể thay đổi, đèn LED RGB và thiết kế gọn nhẹ, phù hợp cho game thủ chuyên nghiệp." },

                        new Product { ProductName = "Bàn phím cơ Razer BlackWidow V3", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 2590000m, UnitsInStock = 70,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Razer BlackWidow V3 mang lại cảm giác gõ êm ái và phản hồi nhanh chóng. Được trang bị đèn LED RGB, switch Green, lý tưởng cho game thủ yêu thích chơi game ban đêm." },

                        new Product { ProductName = "Bàn phím cơ Corsair K95 RGB Platinum", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3890000m, UnitsInStock = 30,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Corsair K95 RGB Platinum với switch Cherry MX và đèn LED RGB có thể tùy chỉnh. Sản phẩm lý tưởng cho game thủ, hỗ trợ macro phím, giúp cải thiện hiệu suất chơi game." },

                        new Product { ProductName = "Bàn phím không dây Logitech K780", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 799000m, UnitsInStock = 80,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bàn phím không dây Logitech K780 với thiết kế thông minh, hỗ trợ kết nối với nhiều thiết bị cùng lúc. Chất liệu cao cấp và trải nghiệm gõ mượt mà." },

                        new Product { ProductName = "Bàn phím cơ SteelSeries Apex Pro", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 4590000m, UnitsInStock = 40,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Bàn phím cơ SteelSeries Apex Pro sử dụng công tắc OmniPoint cho cảm giác gõ tùy chỉnh và tốc độ phản hồi cực nhanh. Đèn LED RGB, thiết kế cao cấp." },

                        new Product { ProductName = "Bàn phím không dây Logitech MX Keys", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1490000m, UnitsInStock = 50,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Logitech MX Keys là bàn phím không dây mang đến trải nghiệm gõ êm ái, có thể kết nối với nhiều thiết bị, phù hợp cho công việc và giải trí." },

                        new Product { ProductName = "Bàn phím gaming MSI Vigor GK50", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1390000m, UnitsInStock = 60,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "MSI Vigor GK50 là bàn phím cơ gaming với switch Cherry MX, thiết kế đèn LED RGB có thể tùy chỉnh. Đảm bảo phản hồi nhanh, hỗ trợ chơi game mượt mà." },

                        new Product { ProductName = "Bàn phím không dây Logitech G915", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 4990000m, UnitsInStock = 40,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Logitech G915 là bàn phím gaming không dây với switch GL có thể thay đổi, đèn LED RGB sáng tạo và thiết kế mỏng, sang trọng." },

                        new Product { ProductName = "Bàn phím cơ HyperX Alloy FPS Pro", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1590000m, UnitsInStock = 70,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "HyperX Alloy FPS Pro là bàn phím cơ compact, thiết kế cho game thủ FPS với khả năng phản hồi nhanh, switch Cherry MX, đèn LED đỏ." },

                        new Product { ProductName = "Bàn phím cơ Aukey KM-G9", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 799000m, UnitsInStock = 90,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Aukey KM-G9 là bàn phím cơ giá phải chăng với đèn LED RGB, switch Blue, cung cấp phản hồi tuyệt vời, lý tưởng cho những game thủ yêu thích hiệu suất và kiểu dáng." },
                // chuột 141 - 150
                        new Product { ProductName = "Chuột gaming Logitech G502 HERO", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1490000m, UnitsInStock = 50,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Chuột gaming Logitech G502 HERO được trang bị cảm biến HERO 16K, mang lại độ chính xác cao và tốc độ phản hồi nhanh. Thiết kế công thái học, đèn LED RGB có thể tùy chỉnh." },

                        new Product { ProductName = "Chuột không dây Logitech MX Master 3", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2490000m, UnitsInStock = 60,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Logitech MX Master 3 là chuột không dây cao cấp, thiết kế công thái học, cảm biến 4000 DPI, kết nối với nhiều thiết bị cùng lúc, thích hợp cho công việc và giải trí." },

                        new Product { ProductName = "Chuột gaming Razer DeathAdder V2", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1190000m, UnitsInStock = 70,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Razer DeathAdder V2 với cảm biến Focus+ 20K DPI, công nghệ HyperSpeed Wireless giúp game thủ có trải nghiệm chơi game mượt mà, phản hồi cực nhanh." },

                        new Product { ProductName = "Chuột có dây SteelSeries Rival 600", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1590000m, UnitsInStock = 40,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "SteelSeries Rival 600 có cảm biến TrueMove3+ với độ chính xác cực cao, thiết kế 2 cảm biến cho khả năng tracking tuyệt vời, thích hợp cho game thủ chuyên nghiệp." },

                        new Product { ProductName = "Chuột không dây Logitech G Pro X Superlight", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3490000m, UnitsInStock = 50,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Logitech G Pro X Superlight là chuột gaming không dây siêu nhẹ, cảm biến HERO 25K, giúp game thủ có độ chính xác cao và tốc độ phản hồi nhanh nhất trong các trận đấu căng thẳng." },

                        new Product { ProductName = "Chuột gaming Corsair Dark Core RGB/SE", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2190000m, UnitsInStock = 30,
                            UnitsOnOrder = 4, Discontinued = false, ProductDescription = "Corsair Dark Core RGB/SE với cảm biến 16K DPI, thiết kế tiện lợi, kết nối không dây hoặc có dây, đèn LED RGB tùy chỉnh cho game thủ muốn một trải nghiệm chơi game tuyệt vời." },

                        new Product { ProductName = "Chuột có dây Razer Naga Trinity", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 2490000m, UnitsInStock = 60,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Razer Naga Trinity với cảm biến 16K DPI, 3 panel nút có thể thay đổi, lý tưởng cho các game thủ MMO, giúp bạn tùy chỉnh dễ dàng các phím tắt trong game." },

                        new Product { ProductName = "Chuột không dây Microsoft Surface Arc", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 799000m, UnitsInStock = 100,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Microsoft Surface Arc là chuột không dây thiết kế mỏng, gọn nhẹ, dễ dàng mang theo, phù hợp với nhu cầu sử dụng văn phòng và di động, cảm giác gõ tốt." },

                        new Product { ProductName = "Chuột gaming HyperX Pulsefire FPS Pro", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1190000m, UnitsInStock = 80,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "HyperX Pulsefire FPS Pro với cảm biến Pixart 16000 DPI, đèn LED RGB, thiết kế công thái học giúp game thủ có trải nghiệm chơi game mượt mà và chính xác." },

                        new Product { ProductName = "Chuột không dây Logitech G703 LIGHTSPEED", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1590000m, UnitsInStock = 50,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Logitech G703 LIGHTSPEED là chuột không dây gaming với cảm biến HERO 16K DPI, đèn LED RGB, giúp game thủ có tốc độ phản hồi cực nhanh và độ chính xác cao." },
                // loa 151 - 160 
                        new Product { ProductName = "Loa Bluetooth JBL Charge 5", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2990000m, UnitsInStock = 50,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Loa Bluetooth JBL Charge 5 với âm thanh mạnh mẽ, thời lượng pin lên đến 20 giờ, thiết kế chống nước IP67, hoàn hảo cho việc nghe nhạc ngoài trời và các buổi tiệc." },

                        new Product { ProductName = "Loa không dây Bose SoundLink Revolve+", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 5990000m, UnitsInStock = 30,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Bose SoundLink Revolve+ cung cấp âm thanh 360 độ tuyệt vời, thiết kế nhỏ gọn, chống nước, thời gian phát nhạc lên đến 16 giờ, phù hợp với cả trong nhà và ngoài trời." },

                        new Product { ProductName = "Loa Bluetooth Sony SRS-XB43", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3590000m, UnitsInStock = 70,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Sony SRS-XB43 với âm thanh Extra Bass mạnh mẽ, thiết kế chống nước IP67, đèn LED nhấp nháy tạo không gian âm nhạc sôi động, thích hợp cho các buổi party." },

                        new Product { ProductName = "Loa máy tính Creative Pebble 2.0", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 599000m, UnitsInStock = 100,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Creative Pebble 2.0 là loa máy tính với âm thanh rõ ràng, bass mạnh mẽ, thiết kế gọn gàng, dễ dàng kết nối qua cổng USB, phù hợp cho không gian văn phòng hoặc phòng ngủ." },

                        new Product { ProductName = "Loa Bluetooth Anker Soundcore Flare 2", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1290000m, UnitsInStock = 90,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Anker Soundcore Flare 2 cung cấp âm thanh 360 độ sống động, đèn LED chớp sáng tạo không gian giải trí tuyệt vời, chống nước IPX7, thời gian sử dụng lên đến 12 giờ." },

                        new Product { ProductName = "Loa Bluetooth Marshall Emberton", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4990000m, UnitsInStock = 40,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Marshall Emberton là loa Bluetooth với âm thanh mạnh mẽ và rõ ràng, thiết kế cổ điển, kết nối dễ dàng với các thiết bị, thời gian sử dụng lên đến 20 giờ." },

                        new Product { ProductName = "Loa ngoài trời Bose SoundLink Micro", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1990000m, UnitsInStock = 80,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Bose SoundLink Micro là loa Bluetooth ngoài trời nhỏ gọn, âm thanh mạnh mẽ, khả năng chống nước IPX7, phù hợp cho các hoạt động ngoài trời và đi du lịch." },

                        new Product { ProductName = "Loa Bluetooth Xiaomi Mi Outdoor", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 799000m, UnitsInStock = 120,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Xiaomi Mi Outdoor với âm thanh mạnh mẽ và bass sâu, thiết kế chống nước, dễ dàng kết nối qua Bluetooth, là sự lựa chọn tuyệt vời cho các chuyến đi dã ngoại và tiệc ngoài trời." },

                        new Product { ProductName = "Loa máy tính Logitech Z313 Speaker System", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 899000m, UnitsInStock = 90,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Logitech Z313 Speaker System với loa siêu trầm, âm thanh rõ ràng, mạnh mẽ, thiết kế nhỏ gọn, dễ dàng kết nối với máy tính hoặc thiết bị di động." },

                        new Product { ProductName = "Loa Bluetooth JBL Flip 5", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1990000m, UnitsInStock = 110,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "JBL Flip 5 là loa Bluetooth với âm thanh stereo sống động, khả năng chống nước IPX7, thời gian sử dụng lên đến 12 giờ, hoàn hảo cho các hoạt động ngoài trời và tiệc tùng." },
                // màn hình 161-170
                        new Product { ProductName = "Màn hình gaming LG 27GN950-B 27 inch 4K UHD", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 11990000m, UnitsInStock = 30,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Màn hình gaming LG 27GN950-B với độ phân giải 4K UHD, tần số quét 144Hz, thời gian phản hồi 1ms, hỗ trợ công nghệ G-Sync giúp mang đến trải nghiệm chơi game mượt mà và sắc nét." },

                        new Product { ProductName = "Màn hình Dell UltraSharp U2720Q 27 inch 4K", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 10490000m, UnitsInStock = 40,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Màn hình Dell UltraSharp U2720Q với độ phân giải 4K, màu sắc chính xác, hỗ trợ USB-C, phù hợp cho các công việc thiết kế đồ họa và làm việc chuyên nghiệp." },

                        new Product { ProductName = "Màn hình Samsung Odyssey G7 27 inch QHD", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 9990000m, UnitsInStock = 50,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Màn hình Samsung Odyssey G7 27 inch với độ phân giải QHD, tần số quét 240Hz, thời gian phản hồi 1ms, thiết kế cong, mang đến trải nghiệm gaming tuyệt vời và hình ảnh sắc nét." },

                        new Product { ProductName = "Màn hình ASUS ProArt PA278QV 27 inch QHD", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 8990000m, UnitsInStock = 70,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "ASUS ProArt PA278QV 27 inch với độ phân giải QHD, màu sắc chuẩn xác cho công việc sáng tạo, tần số quét 75Hz, công nghệ Flicker-Free và Blue Light Filtering bảo vệ mắt." },

                        new Product { ProductName = "Màn hình MSI Optix MAG272C 27 inch Full HD", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 4590000m, UnitsInStock = 80,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Màn hình MSI Optix MAG272C với độ phân giải Full HD, tần số quét 165Hz, thời gian phản hồi 1ms, thiết kế cong 1500R, phù hợp cho game thủ yêu thích độ chính xác và tốc độ." },

                        new Product { ProductName = "Màn hình AOC CQ32G1 31.5 inch QHD Curved", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 6690000m, UnitsInStock = 60,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Màn hình AOC CQ32G1 31.5 inch với độ phân giải QHD, tần số quét 165Hz, cong 1800R, mang lại trải nghiệm chơi game và xem phim cực kỳ mượt mà và sinh động." },

                        new Product { ProductName = "Màn hình BenQ PD2700U 27 inch 4K", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 13990000m, UnitsInStock = 20,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Màn hình BenQ PD2700U 27 inch với độ phân giải 4K, chuẩn màu chính xác, hỗ trợ HDR10, là lựa chọn lý tưởng cho các chuyên gia thiết kế đồ họa và người sáng tạo nội dung." },

                        new Product { ProductName = "Màn hình LG 34WN80C-B 34 inch UltraWide", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 6990000m, UnitsInStock = 45,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Màn hình LG 34WN80C-B 34 inch UltraWide với độ phân giải 2560x1080, thiết kế cong, phù hợp cho công việc đa nhiệm và giải trí, giúp tối ưu hóa không gian làm việc." },

                        new Product { ProductName = "Màn hình ViewSonic VX2458-MHD 24 inch Full HD", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3690000m, UnitsInStock = 100,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Màn hình ViewSonic VX2458-MHD 24 inch Full HD với tần số quét 75Hz, độ phân giải 1080p, thiết kế mỏng nhẹ, dễ dàng lắp đặt cho không gian làm việc hoặc chơi game." },

                        new Product { ProductName = "Màn hình HP 27f 27 inch Full HD", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4890000m, UnitsInStock = 90,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "HP 27f 27 inch Full HD với thiết kế siêu mỏng, tấm nền IPS mang đến hình ảnh sắc nét và màu sắc sống động, lý tưởng cho công việc văn phòng và giải trí." },
                 // micro 171 - 180
                        new Product { ProductName = "Micro thu âm USB Blue Yeti", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2490000m, UnitsInStock = 30,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Micro thu âm USB Blue Yeti với chất lượng âm thanh tuyệt vời, tích hợp 4 chế độ thu âm, dễ dàng kết nối với máy tính, phù hợp cho streamer, podcaster và người làm nội dung." },

                        new Product { ProductName = "Micro thu âm Audio-Technica AT2020", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2390000m, UnitsInStock = 50,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Micro thu âm Audio-Technica AT2020 mang đến chất lượng âm thanh rõ ràng, độ nhạy cao, lý tưởng cho thu âm podcast, nhạc và các buổi livestream." },

                        new Product { ProductName = "Micro không dây Samson XPD2", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3190000m, UnitsInStock = 40,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Micro không dây Samson XPD2 dễ dàng kết nối với thiết bị di động, mang lại âm thanh chất lượng cao, thiết kế nhỏ gọn, phù hợp cho thu âm trực tiếp hoặc trên sân khấu." },

                        new Product { ProductName = "Micro thu âm Razer Seiren X", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1790000m, UnitsInStock = 60,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Micro thu âm Razer Seiren X với khả năng thu âm tinh tế, thiết kế nhỏ gọn và dễ sử dụng, lý tưởng cho các streamer, podcaster và người làm nội dung trực tuyến." },

                        new Product { ProductName = "Micro thu âm Shure SM7B", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 6990000m, UnitsInStock = 20,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Micro thu âm Shure SM7B nổi bật với chất lượng âm thanh tuyệt vời, được ưa chuộng trong phòng thu và các buổi ghi âm podcast chuyên nghiệp." },

                        new Product { ProductName = "Micro thu âm Sony ECM-77B", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 2490000m, UnitsInStock = 50,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Micro thu âm Sony ECM-77B với thiết kế nhỏ gọn, chất lượng thu âm cao, lý tưởng cho thu âm nhạc, phỏng vấn và các ứng dụng video." },

                        new Product { ProductName = "Micro không dây Rode Wireless GO", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 5890000m, UnitsInStock = 35,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Micro không dây Rode Wireless GO nhỏ gọn, chất lượng âm thanh rõ ràng, lý tưởng cho vlog, livestream và phỏng vấn di động." },

                        new Product { ProductName = "Micro thu âm AKG P120", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1490000m, UnitsInStock = 80,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Micro thu âm AKG P120 với chất lượng âm thanh trong trẻo, thích hợp cho thu âm podcast, nhạc và các buổi ghi âm tại nhà." },

                        new Product { ProductName = "Micro thu âm Samson Q2U", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1590000m, UnitsInStock = 70,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Micro thu âm Samson Q2U kết hợp USB và XLR, mang đến chất lượng thu âm tuyệt vời cho podcast, stream và các buổi ghi âm chuyên nghiệp." },

                        new Product { ProductName = "Micro thu âm Blue Snowball iCE", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 1090000m, UnitsInStock = 100,
                            UnitsOnOrder = 15, Discontinued = false, ProductDescription = "Micro thu âm Blue Snowball iCE dễ dàng sử dụng với kết nối USB, mang lại âm thanh rõ ràng, phù hợp cho podcast, livestream và thu âm tại nhà." },
                // tai nghe 181-190
                        new Product { ProductName = "Tai nghe Bluetooth Sony WH-1000XM4", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 7990000m, UnitsInStock = 30,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Tai nghe Bluetooth Sony WH-1000XM4 với công nghệ chống ồn chủ động, chất lượng âm thanh tuyệt vời, pin sử dụng lên đến 30 giờ, mang lại trải nghiệm nghe nhạc tuyệt hảo." },

                        new Product { ProductName = "Tai nghe không dây Apple AirPods Pro", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 6590000m, UnitsInStock = 40,
                            UnitsOnOrder = 6, Discontinued = false, ProductDescription = "Tai nghe không dây Apple AirPods Pro với tính năng chống ồn chủ động, thiết kế nhỏ gọn và thoải mái, âm thanh chất lượng cao, phù hợp cho nghe nhạc và gọi điện." },

                        new Product { ProductName = "Tai nghe có dây JBL Quantum 800", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3990000m, UnitsInStock = 50,
                            UnitsOnOrder = 8, Discontinued = false, ProductDescription = "Tai nghe JBL Quantum 800 có dây với âm thanh vòm 3D, micrô khử tiếng ồn, tần số quét cao và thiết kế thoải mái, lý tưởng cho game thủ." },

                        new Product { ProductName = "Tai nghe Bluetooth Bose QuietComfort 35 II", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 7490000m, UnitsInStock = 30,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Bose QuietComfort 35 II là tai nghe Bluetooth với khả năng chống ồn chủ động, chất lượng âm thanh hoàn hảo, và thiết kế cực kỳ thoải mái cho việc nghe lâu dài." },

                        new Product { ProductName = "Tai nghe có dây Sennheiser HD 600", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 4790000m, UnitsInStock = 25,
                            UnitsOnOrder = 5, Discontinued = false, ProductDescription = "Tai nghe Sennheiser HD 600 với chất lượng âm thanh cực kỳ chi tiết, thiết kế mở giúp tái tạo âm thanh tự nhiên, phù hợp cho những tín đồ audiophile." },

                        new Product { ProductName = "Tai nghe không dây Samsung Galaxy Buds Pro", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 3990000m, UnitsInStock = 70,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Samsung Galaxy Buds Pro với chất lượng âm thanh sống động, chống ồn chủ động, thiết kế nhỏ gọn và tiện dụng, phù hợp cho mọi hoạt động và phong cách sống." },

                        new Product { ProductName = "Tai nghe có dây Razer Kraken X", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 1290000m, UnitsInStock = 60,
                            UnitsOnOrder = 12, Discontinued = false, ProductDescription = "Tai nghe Razer Kraken X có dây với âm thanh 7.1 surround, micrô khử tiếng ồn, thiết kế nhẹ và thoải mái, hoàn hảo cho các game thủ yêu thích sự sống động trong âm thanh." },

                        new Product { ProductName = "Tai nghe Bluetooth Sony WF-1000XM4", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 4490000m, UnitsInStock = 40,
                            UnitsOnOrder = 7, Discontinued = false, ProductDescription = "Sony WF-1000XM4 là tai nghe không dây với khả năng chống ồn chủ động, âm thanh chất lượng cao, và thời gian sử dụng lên đến 8 giờ, mang lại trải nghiệm âm nhạc tuyệt vời." },

                        new Product { ProductName = "Tai nghe không dây Jabra Elite 75t", CategoryID = 4, SupplierID = 1, QuantityPerUnit = "1 kg", UnitPrice = 3590000m, UnitsInStock = 55,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "Tai nghe Jabra Elite 75t không dây với chất lượng âm thanh sắc nét, khả năng chống nước IP55, thời gian sử dụng lên đến 7 giờ, lý tưởng cho tập luyện thể thao và di chuyển." },

                        new Product { ProductName = "Tai nghe có dây SteelSeries Arctis 7", CategoryID = 4, SupplierID = 2, QuantityPerUnit = "1 kg", UnitPrice = 4590000m, UnitsInStock = 50,
                            UnitsOnOrder = 10, Discontinued = false, ProductDescription = "SteelSeries Arctis 7 có dây với âm thanh vòm 7.1, micrô khử tiếng ồn, thiết kế thoải mái, là lựa chọn tuyệt vời cho game thủ và người dùng muốn có trải nghiệm âm thanh chất lượng cao." },

            };
            context.Products.AddRange(products);
            context.SaveChanges();
            var images = new List<Image>();
            for (int i = 1; i <= 190; i++)
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
