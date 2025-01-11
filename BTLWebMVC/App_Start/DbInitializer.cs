using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BTLWebMVC.App_Start
{
    public class DbInitializer : CreateDatabaseIfNotExists<Context>
    {
    }
}