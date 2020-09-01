using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class WorkStreamModel
    {
        [Required]
        [StringLength(50)]
        public string WorkStreamId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkStreamCode { get; set; }

        [Required]
        public int CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        public string AssignWorkId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }

        [Required]
        public byte Status { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public List<string> PerformerBys { get; set; }
        public WorkStream ToObject()
        {
            return new WorkStream
            {
                WorkStreamId = WorkStreamId,
                WorkStreamCode = WorkStreamCode,
                CreateBy = CreateBy,
                CreateDate = CreateDate,
                FromDate = FromDate,
                ToDate = ToDate,
                TaskId = TaskId,
                AssignWorkId = AssignWorkId,
                Description = Description,
                Status = Status,
                ApprovedBy= ApprovedBy,
                ApprovedDate = ApprovedDate,
                PerformerBys = PerformerBys
            };
        }
    }
}