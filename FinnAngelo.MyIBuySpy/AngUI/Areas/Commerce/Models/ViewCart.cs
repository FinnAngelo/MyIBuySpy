using System;
using System.Collections.Generic;

#nullable disable

namespace FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Models
{
    public partial class ViewCart
    {
        public int ProductId { get; set; }
        public string ModelNumber { get; set; }
        public string ModelName { get; set; }
        public decimal UnitCost { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; }
    }
}
