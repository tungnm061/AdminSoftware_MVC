using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminSoftware.Areas.Sale.Models
{
    public class OrderDetailModel
    {
        public string OrderDetailId { get; set; }

        public long ProductId { get; set; }

        public int Quantity { get; set; }

        public byte Size { get; set; }

        public byte Color { get; set; }

        public decimal UnitPrince { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}