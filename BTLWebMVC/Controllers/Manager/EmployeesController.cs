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
    public class EmployeesController : Controller
    {
        private Context db = new Context();


        [HttpGet]
        // GET: Employees
        public ActionResult Index()
        {

            //// lấy accountDID từ session L
            //var accountId = Session["AccountId"]?.ToString();
            //if (string.IsNullOrEmpty(accountId))
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            //int id = Convert.ToInt32(accountId);
            //var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
            //if(account == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            //// lưu vào viewbag để sd torng view
            //ViewBag.UserRole = account.Role;

            //// ko phải admin hay employee thì chuyễn trang
            //if( account.Role != "Admin" && account.Role != "Employee")
            //{
            //    return RedirectToAction("AccessDenied", "Home");
            //}

            var employees = db.Employees.Include(e => e.Account).ToList();
            return View(employees);
        }

      
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username");
            return View();
        }

  
        [HttpPost]
 
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,FirstName,LastName,BirthDate,HireDate,Address,City,PostalCode,Country,Phone,Email,AccountID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", employee.AccountID);
            return View(employee);
        }


       
        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", employee.AccountID);
            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
 
        public ActionResult Edit([Bind(Include = "EmployeeID,FirstName,LastName,BirthDate,HireDate,Address,City,PostalCode,Country,Phone,Email,AccountID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "AccountID", "Username", employee.AccountID);
            return View(employee);
        }

        // xoa nhan vien
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                
            }
            Employee employee = db.Employees.Find(id);
            if(employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

      
        public ActionResult Lock(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            var employee = db.Employees.Find(id);
            if(employee == null)
            {
                return HttpNotFound();
            }
            var account = db.Accounts.Find(employee.AccountID)
                ;
            if(account == null)
            {
                return HttpNotFound();
            }
            account.IsLock = true;
            db.SaveChanges();
            return RedirectToAction("Index");
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
