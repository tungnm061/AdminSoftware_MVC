using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class IncurredSalaryModel
    {
        public string IncurredSalaryId { get; set; }
        public long EmployeeId { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime SubmitDate { get; set; }
        public string Description { get; set; }

        public IncurredSalary ToObject()
        {
            return new IncurredSalary
            {
                EmployeeId = EmployeeId,
                CreateDate = CreateDate,
                Description = Description,
                FullName = Description,
                CreateBy = CreateBy,
                EmployeeCode = Description,
                DepartmentId = CreateBy,
                Amount = Amount,
                IncurredSalaryId = IncurredSalaryId,
                SubmitDate = SubmitDate,
                Title = Title
            };
        }
    }
}