using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class OrderDetail
    {
        [LocalizeRequired]
        public string OrderDetailId { get; set; }

        [LocalizeRequired]
        [DisplayName("Sản phẩm")]
        public long ProductId { get; set; }

        [LocalizeRequired]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [LocalizeRequired]
        [DisplayName("Kích thước")]
        public byte Size { get; set; }

        [LocalizeRequired]
        [DisplayName("Màu sắc")]
        public byte Color { get; set; }

        public decimal UnitPrince { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

    }
}
