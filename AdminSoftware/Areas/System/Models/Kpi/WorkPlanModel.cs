using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class WorkPlanModel
    {
        [Required]
        [StringLength(50)]
        public string WorkPlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkPlanCode { get; set; }

        [Required]
        public int CreateBy { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public int? ConfirmedBy { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public WorkPlan ToObject()
        {
            return new WorkPlan
            {
                WorkPlanId = WorkPlanId,
                WorkPlanCode = WorkPlanCode,
                CreateBy = CreateBy,
                FromDate = FromDate,
                ToDate = ToDate,
                CreateDate = CreateDate,
                Description = Description,
                ConfirmedBy = ConfirmedBy,
                ApprovedBy = ApprovedBy,
                ConfirmedDate = ConfirmedDate,
                ApprovedDate = ApprovedDate
            };
        }
    }
}