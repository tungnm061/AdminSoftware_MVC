using System;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class HolidayDetailModel
    {
        public string HolidayDetailId { get; set; }
        public DateTime DateDay { get; set; }
        public decimal NumberDays { get; set; }
        public decimal Permission { get; set; }
        public decimal PercentSalary { get; set; }
        public decimal ToTalDays { get; set; }
        public string EmployeeHolidayId { get; set; }

        public HolidayDetail ToObject()
        {
            return new HolidayDetail
            {
                HolidayDetailId = HolidayDetailId,
                DateDay = DateDay,
                NumberDays = NumberDays,
                Permission = Permission,
                PercentSalary = PercentSalary,
                ToTalDays = ToTalDays,
                EmployeeHolidayId = EmployeeHolidayId
            };
        }
    }
}