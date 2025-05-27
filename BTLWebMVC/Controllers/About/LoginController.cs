using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;
using System.Text.RegularExpressions;

namespace BTLWebMVC.Controllers
{
    public class LoginController : Controller
    {
        private Context db = new Context();
        public ActionResult Logout()
        {
            Session.Clear();

            TempData["SuccessMessage"] = "Bạn đã đăng xuất thành công!";

            return RedirectToAction("Index", "Home");
        }
       
        public ActionResult ResetPassword(string token)
        {
            var account = db.Accounts.FirstOrDefault(a => a.TokenCode == token);
            if (account == null)
            {
                ViewBag.Message = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn. Vui lòng yêu cầu đặt lại mật khẩu mới!";
                ViewBag.Token = null; 
            }
            else
            {
                ViewBag.Token = token;
            }
            return View();
        }

        [HttpPost]
        public JsonResult ResetPasswordConfirm(string token, string newPassword)
        {
            var account = db.Accounts.FirstOrDefault(a => a.TokenCode == token);
            if (account == null)
            {
                return Json(new { success = false, message = "Liên kết không hợp lệ!" });
            }

            if (newPassword.Length < 6)
            {
                return Json(new { success = false, message = "Vui lòng nhập nhiều hơn 6 ký tự!" });
            }
            account.Password = newPassword;
            account.TokenCode = null;
            db.SaveChanges();

            return Json(new { success = true, message = "Mật khẩu đã được cập nhật!" });
        }
        [HttpPost]
        public JsonResult Forgotpassword(string username, string email, string phoneNumber)
        {
            if (!ValidateUserInformation(username, email, phoneNumber))
            {
                return Json(new { success = false, message = "Thông tin không hợp lệ!" });
            }

            SendPasswordResetEmail(email, username);
            return Json(new { success = true, message = "Yêu cầu đã được gửi, vui lòng kiểm tra email!" });
        }
        private bool ValidateUserInformation(string username, string email, string phoneNumber)
        {

            var user = db.Accounts.FirstOrDefault(a => a.Username == username && a.Email == email);

            if (user != null)
            {
                int accountId = user.AccountID;

                var customer = db.Customers.FirstOrDefault(c => c.AccountID == accountId && c.Phone == phoneNumber);
                var employee = db.Employees.FirstOrDefault(e => e.AccountID == accountId && e.Phone == phoneNumber);

                if (customer != null || employee != null)
                {
                    return true;
                }
            }
            return false;
        }
        
        private void SendPasswordResetEmail(string email, string username)
        {
            string token = Guid.NewGuid().ToString();
            DateTime expirationTime = DateTime.Now.AddMinutes(10);

            var account = db.Accounts.FirstOrDefault(a => a.Email == email && a.Username == username);
            if (account == null) return;

            account.TokenCode = token;
            db.SaveChanges();

            string resetLink = $"{Request.Url.GetLeftPart(UriPartial.Authority)}/Login/ResetPassword?token=" + token;

            string body = $@"
            <html>
            <body>
                <p>Xin chào,</p>
                <p>Bạn đã yêu cầu đặt lại mật khẩu. Nhấp vào liên kết bên dưới để tiếp tục:</p>
                <p><a href='{resetLink}' style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none;'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>
            </body>
            </html>";

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("dhao3017@gmail.com", "Cửa hàng HPH"),
                Subject = "Đặt lại mật khẩu",
                Body = body,
                IsBodyHtml = true
            };
            mail.To.Add(email);

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("dhao3017@gmail.com", "ttpm evrc vlcp qnme"),
                EnableSsl = true
            };

            smtp.Send(mail);
        }
        [HttpPost]
        public JsonResult ChangePassword(string oldPassword, string newPassword, string newReEnterPassword) 
        {
            var accountId = Session["AccountId"];
            if (accountId == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }
            int id = Convert.ToInt32(accountId);
            var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            if (account == null)
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập lại!" });
            }
            if (account.Password != oldPassword)
            {
                return Json(new { success = false, message = "Mật khẩu hiện tại không chính xác!" });
            }
            if (newPassword != newReEnterPassword)
            {
                return Json(new { success = false, message = "Mật khẩu mới và mật khẩu nhập lại không chính xác!" });
            }
            if (account.Password == newPassword)
            {
                return Json(new { success = false, message = "Vui lòng nhập mật khẩu mới và mật khẩu cũ khác nhau!" });
            }
            if (newPassword.Length < 6 || newReEnterPassword.Length < 6)
            {
                return Json(new { success = false, message = "Vui lòng nhập mật khẩu mới dài hơn 6 ký tự!" });
            }
            account.Password = newPassword;
            db.SaveChanges();
            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
        }
        [HttpPost]
        public JsonResult Register(string newUsername, string newPassword, string newName, string newEmail, string newPhone)
        {
            // Xóa khoảng trắng thừa
            newUsername = newUsername?.Trim();
            newPassword = newPassword?.Trim();
            newName = newName?.Trim();
            newEmail = newEmail?.Trim();
            newPhone = newPhone?.Trim();

            // Kiểm tra các trường không được để trống
            if (string.IsNullOrEmpty(newUsername))
            {
                return Json(new { success = false, message = "Tên đăng nhập không được để trống." });
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return Json(new { success = false, message = "Mật khẩu không được để trống." });
            }
            if (string.IsNullOrEmpty(newName))
            {
                return Json(new { success = false, message = "Tên không được để trống." });
            }
            if (string.IsNullOrEmpty(newEmail))
            {
                return Json(new { success = false, message = "Email không được để trống." });
            }
            if (string.IsNullOrEmpty(newPhone))
            {
                return Json(new { success = false, message = "Số điện thoại không được để trống." });
            }

            // Kiểm tra định dạng tên người dùng
            if (!Regex.IsMatch(newUsername, @"^[a-zA-Z0-9_.]{3,50}$"))
            {
                return Json(new { success = false, message = "Tên đăng nhập chỉ chứa chữ cái, số, dấu chấm hoặc gạch dưới, độ dài từ 3 đến 50 ký tự." });
            }

            // Kiểm tra tên người dùng đã tồn tại
            if (db.Accounts.Any(a => a.Username == newUsername))
            {
                return Json(new { success = false, message = "Tên đăng nhập đã tồn tại." });
            }

            // Kiểm tra định dạng mật khẩu
            if (newPassword.Length < 6 || newPassword.Length > 100)
            {
                return Json(new { success = false, message = "Mật khẩu phải có độ dài từ 6 đến 100 ký tự." });
            }
            if (!Regex.IsMatch(newPassword, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,100}$"))
            {
                return Json(new { success = false, message = "Mật khẩu phải chứa ít nhất một chữ cái in hoa, một chữ cái thường, một số và một ký tự đặc biệt." });
            }

            // Kiểm tra định dạng email
            if (newEmail.Length > 100 || !Regex.IsMatch(newEmail, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return Json(new { success = false, message = "Email không đúng định dạng." });
            }

            // Kiểm tra email đã tồn tại
            if (db.Accounts.Any(a => a.Email == newEmail))
            {
                return Json(new { success = false, message = "Email đã được sử dụng." });
            }

            // Kiểm tra định dạng số điện thoại
            if (newPhone.Length > 15 || !Regex.IsMatch(newPhone, @"^(?:\+84|0)(3|5|7|8|9)\d{8}$"))
            {
                return Json(new { success = false, message = "Số điện thoại không đúng định dạng (VD: +849xxxxxxxx hoặc 09xxxxxxxx)." });
            }

            // Kiểm tra định dạng tên
            if (newName.Length < 2 || newName.Length > 100 || !Regex.IsMatch(newName, @"^[a-zA-ZÀ-ỹ\s]+$"))
            {
                return Json(new { success = false, message = "Tên chỉ chứa chữ cái và dấu cách, độ dài từ 2 đến 100 ký tự." });
            }

            // Tạo tài khoản
            var account = new Account
            {
                Username = newUsername,
                Password = newPassword, // Lưu ý: Nên mã hóa mật khẩu trước khi lưu
                Email = newEmail,
                ProfileImage = "profile.jpg",
                CreatedDate = DateTime.Now,
                IsLock = false,
                Role = "Customer"
            };
            db.Accounts.Add(account);
            db.SaveChanges();

            // Tạo khách hàng
            var customer = new Customer
            {
                CustomerName = newName,
                ContactName = newName,
                Phone = newPhone,
                Email = newEmail,
                AccountID = account.AccountID
            };
            db.Customers.Add(customer);
            db.SaveChanges();

            return Json(new { success = true, message = "Đăng ký thành công!" });
        }
        [HttpPost]
        public JsonResult Loginnew(string username, string password)
        {
            var account = db.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);

            if (account == null)
            {
                return Json(new { success = false, message = "Tên đăng nhập hoặc mật khẩu không đúng!" });
            }
            if (account.IsLock)
            {
                return Json(new { success = false, message = "Tài Khoản bị khóa vui lòng liên hệ quản lý!" });
            }
            Session["AccountId"] = account.AccountID;
            Session["Role"] = account.Role;

            if (account.Role == "Admin" || account.Role == "Employee")
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "HomeManager", new { id = account.AccountID }) });
            }
            else
            {
                return Json(new { success = true, reload = true });
            }
        }
    }
}