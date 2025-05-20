using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BTLWebMVC.Models;
using BTLWebMVC.App_Start;

namespace BTLWebMVC.Controllers.User
{
    public class UserAcountController : Controller
    {
        Context db = new Context();
        // GET: UserAcount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DisplayUserInfo(int accountId)
        {
            // Retrieve the account information from the database based on the account ID
            Account account = GetAccountFromDatabase(accountId);

            // Pass the account information to the view
            return View(account);
        }

        public ActionResult UpdateUserInfo(Account updatedAccount)
        {
            // Update the account information in the database
            UpdateAccountInDatabase(updatedAccount);

            // Redirect to the user account page
            return RedirectToAction("Index");
        }

        private Account GetAccountFromDatabase(int accountId)
        {
            // Code to retrieve the account information from the database
            // Replace this with your actual implementation
            // For example:
            Account account = db.Accounts.FirstOrDefault(a => a.AccountID == accountId);
            return account;
        }

        private void UpdateAccountInDatabase(Account updatedAccount)
        {
            // Code to update the account information in the database
            // Replace this with your actual implementation
            // For example:
            Account existingAccount = db.Accounts.FirstOrDefault(a => a.AccountID == updatedAccount.AccountID);
            if (existingAccount != null)
            {
                existingAccount.Username = updatedAccount.Username;
                existingAccount.Password = updatedAccount.Password;
                existingAccount.Email = updatedAccount.Email;
                existingAccount.ProfileImage = updatedAccount.ProfileImage;
                existingAccount.Role = updatedAccount.Role;
                existingAccount.IsLock = updatedAccount.IsLock;
                existingAccount.TokenCode = updatedAccount.TokenCode;

                db.SaveChanges();
            }
        }
    }
} 