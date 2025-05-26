using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;
using System.IO;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BTLWebMVC.Controllers.User
{
    public class UserAcountController : Controller
    {
        Context db = new Context();

        // GET: UserAcount
        public ActionResult Index()
        {
            if (Session["AccountId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int accountId = Convert.ToInt32(Session["AccountId"]);
            var customer = db.Customers
                .Include("Account")
                .FirstOrDefault(c => c.AccountID == accountId);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Update user information
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Customer model, HttpPostedFileBase profileImage)
        {
            if (Session["AccountId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                int accountId = Convert.ToInt32(Session["AccountId"]);
                var customer = db.Customers
                    .Include("Account")
                    .FirstOrDefault(c => c.AccountID == accountId);

                if (customer == null)
                {
                    return HttpNotFound();
                }

                // Update customer information
                customer.CustomerName = model.CustomerName;
                customer.ContactName = model.ContactName;
                customer.Address = model.Address;
                customer.City = model.City;
                customer.PostalCode = model.PostalCode;
                customer.Country = model.Country;
                customer.Phone = model.Phone;
                customer.Email = model.Email;

                // Handle profile image upload
                if (profileImage != null && profileImage.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(profileImage.FileName) + "_" + DateTime.Now.Ticks + Path.GetExtension(profileImage.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/accountImages"), fileName);
                    profileImage.SaveAs(path);
                    customer.Account.ProfileImage = fileName;
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                return RedirectToAction("Index");
            }

            return View("Index", model);
        }
    }
}