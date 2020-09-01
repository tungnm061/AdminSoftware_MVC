using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class InsuranceModel
    {
        [Required]
        public long InsuranceId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string InsuranceNumber { get; set; }

        [Required]
        public DateTime SubscriptionDate { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int? MonthBefore { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }

        public Insurance ToObject()
        {
            return new Insurance
            {
                Description = Description,
                EmployeeId = EmployeeId,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                FullName = FullName,
                EmployeeCode = EmployeeCode,
                IsActive = IsActive,
                DepartmentId = DepartmentId,
                CityId = CityId,
                InsuranceId = InsuranceId,
                InsuranceNumber = InsuranceNumber,
                MonthBefore = MonthBefore,
                SubscriptionDate = SubscriptionDate
            };
        }
    }
}