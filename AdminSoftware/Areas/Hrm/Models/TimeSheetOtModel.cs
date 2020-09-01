using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class TimeSheetOtModel
    {
        [Required]
        public string TimeSheetOtId { get; set; }
        [Required]
        public DateTime DayDate { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public decimal Hours { get; set; }
        [Required]
        public decimal CoefficientPoint { get; set; }
        public decimal DayPoints { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }

        public TimeSheetOt ToObject()
        {
            return new TimeSheetOt
            {
                TimeSheetOtId = TimeSheetOtId,
                DayDate = DayDate,
                EmployeeId = EmployeeId,
                Hours = Hours,
                CoefficientPoint = CoefficientPoint,
                DayPoints = DayPoints,
                CreateDate = CreateDate,
                Description = Description
            };
        }
    }
}