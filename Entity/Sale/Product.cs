using System;
using System.ComponentModel;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class Product
    {
        public long ProductId { get; set; }
        [LocalizeRequired]
        [DisplayName("Mã sản phẩm")]
        public string ProductCode { get; set; }
        [LocalizeStringLength(255)]
        [LocalizeRequired]
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; }
        [LocalizeRequired]
        [DisplayName("Đơn giá")]
        public decimal Price { get; set; }
        [LocalizeRequired]
        [DisplayName("Số lượng")]
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public decimal ToTalQuantity { get; set; }
    }
}