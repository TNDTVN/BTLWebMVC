using PagedList;

namespace BTLWebMVC.Models
{
    public class OrderViewModel
    {
        public IPagedList<Order> PendingOrders { get; set; }
        public IPagedList<Order> ApprovedOrders { get; set; }
        public IPagedList<Order> CancelledOrders { get; set; }
    }
}