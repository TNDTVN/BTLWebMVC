using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BTLWebMVC.Models
{
    public class Notification
    {
        public int NotificationID { get; set; } 
        public string Title { get; set; } 
        public string Content { get; set; } 
        public int SenderID { get; set; } 
        public int ReceiverID { get; set; }
        public DateTime CreatedDate { get; set; } 
        public bool IsRead { get; set; } 

        public virtual Account Sender { get; set; } 
        public virtual Account Receiver { get; set; }
    }
}
