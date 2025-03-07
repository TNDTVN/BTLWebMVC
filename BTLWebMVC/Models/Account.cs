using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebMVC.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Role { get; set; }
        public bool IsLock { get; set; }
        public string TokenCode { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}