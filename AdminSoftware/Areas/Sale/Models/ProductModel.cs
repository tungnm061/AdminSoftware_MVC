using System;
using System.ComponentModel.DataAnnotations;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class ProductModel
    {
        [Required]
        public long ProductId { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        public Product ToObject()
        {
            return new Product
            {
                ProductId = ProductId,
                ProductCode = ProductCode,
                ProductName = ProductName,
                Price = Price,
                Quantity = Quantity,
                IsActive = IsActive,
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy
            };
        }
    }
}