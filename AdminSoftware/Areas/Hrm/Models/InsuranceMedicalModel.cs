using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class InsuranceMedicalModel
    {
        [Required]
        [StringLength(50)]
        public string InsuranceMedicalId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string InsuranceMedicalNumber { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime ExpiredDate { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int MedicalId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }

        public InsuranceMedical ToObject()
        {
            return new InsuranceMedical
            {
                Description = Description,
                EmployeeId = EmployeeId,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                FullName = FullName,
                EmployeeCode = EmployeeCode,
                Amount = Amount,
                CityId = CityId,
                DepartmentId = DepartmentId,
                ExpiredDate = ExpiredDate,
                InsuranceMedicalId = InsuranceMedicalId,
                InsuranceMedicalNumber = InsuranceMedicalNumber,
                MedicalId = MedicalId,
                StartDate = StartDate
            };
        }
    }
}