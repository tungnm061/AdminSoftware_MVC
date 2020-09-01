using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class JobChangeModel
    {
        [Required]
        public string JobChangeId { get; set; }

        [Required]
        [StringLength(50)]
        public string JobChangeCode { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long FromDepartmentId { get; set; }

        [Required]
        public long ToDepartmentId { get; set; }

        public int? FromPositionId { get; set; }
        public int? ToPositionId { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        [StringLength(255)]
        public string JobChangeFile { get; set; }

        [StringLength(50)]
        public string JobChangeNumber { get; set; }

        public JobChange ToObject()
        {
            return new JobChange
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                EmployeeId = EmployeeId,
                Reason = Reason,
                JobChangeCode = JobChangeCode,
                JobChangeId = JobChangeId,
                FromDepartmentId = FromDepartmentId,
                FromPositionId = FromPositionId,
                JobChangeFile = JobChangeFile,
                JobChangeNumber = JobChangeNumber,
                ToDepartmentId = ToDepartmentId,
                ToPositionId = ToPositionId
            };
        }
    }
}