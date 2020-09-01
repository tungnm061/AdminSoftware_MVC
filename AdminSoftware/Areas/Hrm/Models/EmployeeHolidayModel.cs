using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class EmployeeHolidayModel
    {
        [Required]
        public string EmployeeHolidayId { get; set; }
        [Required]
        public int HolidayReasonId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        public EmployeeHoliday ToObject()
        {
            return new EmployeeHoliday
            {
                EmployeeHolidayId = EmployeeHolidayId,
                HolidayReasonId = HolidayReasonId,
                FromDate = FromDate,
                ToDate = ToDate,
                Description = Description,
                EmployeeId = EmployeeId,
                CreateDate = CreateDate
            };
        }
    }
}