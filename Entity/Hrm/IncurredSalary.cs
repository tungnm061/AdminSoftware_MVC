using System;

namespace Entity.Hrm
{
    public class IncurredSalary
    {
        public string IncurredSalaryId { get; set; }
        public long EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
    }
}