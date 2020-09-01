using System;

namespace Entity.Hrm
{
    public class Salary
    {
        public string SalaryId { get; set; }
        public long EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal BasicCoefficient { get; set; }
        public decimal ProfessionalCoefficient { get; set; }
        public decimal ResponsibilityCoefficient { get; set; }
        public decimal PercentProfessional { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long DepartmentId { get; set; }
    }
}