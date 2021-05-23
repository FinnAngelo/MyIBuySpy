using System;
using System.Collections.Generic;

#nullable disable

namespace FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
