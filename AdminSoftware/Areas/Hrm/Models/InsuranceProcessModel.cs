using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class InsuranceProcessModel
    {
        [Required]
        [StringLength(50)]
        public string InsuranceProcessId { get; set; }

        [Required]
        public long InsuranceId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string InsuranceNumber { get; set; }

        public InsuranceProcess ToObject()
        {
            return new InsuranceProcess
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                FullName = FullName,
                EmployeeCode = EmployeeCode,
                Amount = Amount,
                InsuranceId = InsuranceId,
                InsuranceNumber = InsuranceNumber,
                FromDate = FromDate,
                InsuranceProcessId = InsuranceProcessId,
                ToDate = ToDate
            };
        }
    }
}