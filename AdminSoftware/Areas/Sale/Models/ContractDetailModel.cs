using System.ComponentModel.DataAnnotations;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class ContractDetailModel
    {
        [Required]
        public string ContractDetailId { get; set; }

        public long ContractId { get; set; }

        [Required]
        public long ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ContractDetail ToObject()
        {
            return new ContractDetail
            {
                ContractDetailId = ContractDetailId,
                ContractId = ContractId,
                ProductId = ProductId,
                Quantity = Quantity
            };
        }
    }
}