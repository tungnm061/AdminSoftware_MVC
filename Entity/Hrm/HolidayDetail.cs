using System;

namespace Entity.Hrm
{
    public class HolidayDetail
    {
        public string HolidayDetailId { get; set; }
        public DateTime DateDay { get; set; }
        public decimal NumberDays { get; set; }
        public decimal Permission { get; set; }
        public decimal PercentSalary { get; set; }
        public decimal ToTalDays { get; set; }
        public string EmployeeHolidayId { get; set; }
    }
}