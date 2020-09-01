using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class TimeSheetModel
    {
        [Required]
        public string TimeSheetId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public DateTime TimeSheetDate { get; set; }
        public DateTime? Checkin { get; set; }
        public DateTime? Checkout { get; set; }

        public TimeSheet ToObject()
        {
            return new TimeSheet
            {
                TimeSheetId = TimeSheetId,
                EmployeeId = EmployeeId,
                TimeSheetDate = TimeSheetDate,
                Checkin = Checkin,
                Checkout = Checkout
            };
        }
    }
}