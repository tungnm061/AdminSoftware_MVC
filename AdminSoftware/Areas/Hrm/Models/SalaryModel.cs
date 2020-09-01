using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class SalaryModel
    {
        [Required]
        [StringLength(50)]
        public string SalaryId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public decimal BasicSalary { get; set; }

        [Required]
        public decimal BasicCoefficient { get; set; }

        [Required]
        public decimal ProfessionalCoefficient { get; set; }

        [Required]
        public decimal ResponsibilityCoefficient { get; set; }

        [Required]
        public decimal PercentProfessional { get; set; }

        [Required]
        public DateTime ApplyDate { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        public Salary ToObject()
        {
            return new Salary
            {
                EmployeeId = EmployeeId,
                FullName = SalaryId,
                SalaryId = SalaryId,
                CreateDate = CreateDate,
                ApplyDate = ApplyDate,
                BasicCoefficient = BasicCoefficient,
                BasicSalary = BasicSalary,
                CreateBy = CreateBy,
                PercentProfessional = PercentProfessional,
                ProfessionalCoefficient = ProfessionalCoefficient,
                ResponsibilityCoefficient = ResponsibilityCoefficient
            };
        }
    }
}