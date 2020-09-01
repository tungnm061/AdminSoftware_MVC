using System;
using System.Collections.Generic;

namespace Entity.Hrm
{
    public class EmployeeHoliday
    {
        public string EmployeeHolidayId { get; set; }
        public int HolidayReasonId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        public long EmployeeId { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string DepartmentName { get; set; }
        public decimal SalaryPercent { get; set; }
        public string ReasonName { get; set; }

        public List<HolidayDetail> HolidayDetails { get; set; }

    }


}