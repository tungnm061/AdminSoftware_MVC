using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class RecruitPlanModel
    {
        [Required]
        public long RecruitPlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string RecruitPlanCode { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public long DepartmentId { get; set; }

        [Required]
        public int PositionId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [StringLength(4000)]
        public string Requirements { get; set; }

        [Required]
        [StringLength(50)]
        public string ChanelIds { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public RecruitPlan ToObject()
        {
            return new RecruitPlan
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                IsActive = IsActive,
                Quantity = Quantity,
                FromDate = FromDate,
                ToDate = ToDate,
                Title = Title,
                DepartmentId = DepartmentId,
                PositionId = PositionId,
                ChanelIds = ChanelIds,
                RecruitPlanCode = RecruitPlanCode,
                RecruitPlanId = RecruitPlanId,
                Requirements = Requirements
            };
        }
    }
}