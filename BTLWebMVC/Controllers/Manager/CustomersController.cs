using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.App_Start;
using BTLWebMVC.Models;

namespace BTLWebMVC.Controllers.Manager
{
    public class CustomersController : Controller
    {
        private Context db = new Context();

        // GET: Customers
        public ActionResult Index()
        {
            ViewBag.CurrentPage = "Customers";
            var customers = db.Customers.Include(c => c.Account).ToList();
            return View(customers);
        }




        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CurrentPage = "Customers";
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username"); 
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,CustomerName,ContactName,Address,City,PostalCode,Country,Phone,Email,AccountID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi tạo khách hàng: " + ex.Message);
                }
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID); 
            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }


        // sua
        [HttpGet]
       public ActionResult Edit(int ? id)
        {

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if(customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.CurrentPage = "Customers";
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID);

            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,CustomerName,ContactName,Address,City,PostalCode,Country,Phone,Email,AccountID")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật khách hàng: " + ex.Message);
                }
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", customer.AccountID);
            ViewBag.CurrentPage = "Customers";
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
