using System;

namespace Entity.Hrm
{
    public class TimeSheetOt
    {
        public string TimeSheetOtId { get; set; }
        public DateTime DayDate { get; set; }
        public long EmployeeId { get; set; }
        public decimal Hours { get; set; }
        public decimal CoefficientPoint { get; set; }
        public decimal DayPoints { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
    }
}