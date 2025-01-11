using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebMVC.Models
{
    public class Image
    {
        public int ImageID { get; set; }
        public int ProductID { get; set; }
        public string ImageName { get; set; }

        public virtual Product Product { get; set; }
    }
}