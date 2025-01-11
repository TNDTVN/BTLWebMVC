using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BTLWebMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                Database.SetInitializer(new App_Start.DbInitializer());
                using (var context = new App_Start.Context())
                {
                    context.Database.Initialize(force: true);
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(Server.MapPath("~/ErrorLog.txt"), ex.ToString());
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
