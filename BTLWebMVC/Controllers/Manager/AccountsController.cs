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
    public class AccountsController : Controller
    {
        private Context db = new Context();


        // GET: Accounts
        public ActionResult Index()
        {
            var accounts = db.Accounts.ToList();
            return View(accounts);
        }
    }
}