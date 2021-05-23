using System;
using System.Collections.Generic;

#nullable disable

namespace FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Models
{
    public partial class ViewAlsoPurchased
    {
        public int? ProductId { get; set; }
        public string ModelName { get; set; }
        public int? TotalNumPurchased { get; set; }
        public int? OrderId { get; set; }
        public int ProductsProductId { get; set; }
    }
}
