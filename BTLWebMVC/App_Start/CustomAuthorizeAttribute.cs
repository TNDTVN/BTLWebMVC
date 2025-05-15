using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BTLWebMVC.Models;
using System.Web.Mvc;

namespace BTLWebMVC.App_Start
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // han chế các chức năn theo quyền

        private readonly string[] allowedRoles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            allowedRoles = roles;

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var accountId = httpContext.Session["AccountId"]?.ToString();
            if (string.IsNullOrEmpty(accountId))
            {
                return false;
            }

            int id = Convert.ToInt32(accountId);
            using (var db = new Context())
            {
                var account = db.Accounts.FirstOrDefault(a => a.AccountID == id);
                if (account == null)
                {
                    return false;
                }
                return allowedRoles.Contains(account.Role);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "AccessDenied" }
                });
        }
    }
}