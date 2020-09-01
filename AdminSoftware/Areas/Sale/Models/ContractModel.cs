using System;
using System.ComponentModel.DataAnnotations;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class ContractModel
    {
        [Required]
        public long ContractId { get; set; }

        [Required]
        [StringLength(50)]
        public string ContractCode { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public int? EmployeeId { get; set; }

        [Required]
        public int CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int Status { get; set; }
        public string ContractNumber { get; set; }

        public Contract ToObject()
        {
            return new Contract
            {
                ContractId = ContractId,
                ContractCode = ContractCode,
                CustomerId = CustomerId,
                EmployeeId = EmployeeId,
                CreateBy = CreateBy,
                CreateDate = CreateDate,
                TotalPrice = TotalPrice,
                Description = Description,
                Status = Status,
                ContractNumber = ContractNumber
            };
        }
    }
}