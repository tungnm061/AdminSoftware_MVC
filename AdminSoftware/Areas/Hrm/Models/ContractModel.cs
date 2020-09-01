using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class ContractModel
    {
        [Required]
        public string ContractId { get; set; }

        [Required]
        [StringLength(50)]
        public string ContractCode { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int ContractTypeId { get; set; }

        [StringLength(255)]
        public string ContractFile { get; set; }

        [StringLength(255)]
        public string ContractOthorFile { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public Contract ToObject()
        {
            return new Contract
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                EndDate = EndDate,
                StartDate = StartDate,
                ContractTypeId = ContractTypeId,
                EmployeeId = EmployeeId,
                ContractCode = ContractCode,
                ContractFile = ContractFile,
                ContractId = ContractId,
                ContractOthorFile = ContractOthorFile
            };
        }
    }
}