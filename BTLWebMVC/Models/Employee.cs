using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BTLWebMVC.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Họ là bắt buộc")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Tên là bắt buộc")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Ngày thuê là bắt buộc")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

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