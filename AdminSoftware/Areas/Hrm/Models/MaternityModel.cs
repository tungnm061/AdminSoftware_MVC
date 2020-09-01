using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class MaternityModel
    {
        public string MaternityId { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
        [Required]
        public string RelaxStartTime { get; set; }
        [Required]
        public string RelaxEndTime { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int CreateBy { get; set; }
        public string Description { get; set; }


        public Maternity ToObject()
        {
            return new Maternity
            {
                MaternityId = MaternityId,
                EmployeeId = EmployeeId,
                FromDate = FromDate,
                ToDate = ToDate,
                StartTime = StartTime,
                EndTime = EndTime,
                RelaxStartTime = RelaxStartTime,
                RelaxEndTime = RelaxEndTime,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                Description = Description

            };
        }
    }
}