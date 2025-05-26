using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTLWebMVC.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }

        [Required(ErrorMessage = "Tên khách hàng là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự")]
        public string CustomerName { get; set; }

        [StringLength(100, ErrorMessage = "Tên liên hệ không được vượt quá 100 ký tự")]
        public string ContactName { get; set; }

        [StringLength(100, ErrorMessage = "Địa chỉ không được vượt quá 100 ký tự")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "Thành phố không được vượt quá 50 ký tự")]
        public string City { get; set; }

        [StringLength(10, ErrorMessage = "Mã bưu điện không được vượt quá 10 ký tự")]
        public string PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Quốc gia không được vượt quá 50 ký tự")]
        public string Country { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        public int AccountID { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}