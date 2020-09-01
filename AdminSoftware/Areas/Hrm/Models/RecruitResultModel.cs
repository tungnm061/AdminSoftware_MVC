using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class RecruitResultModel
    {
        [Required]
        [StringLength(50)]
        public string RecruitResultId { get; set; }

        [Required]
        [StringLength(50)]
        public string ApplicantId { get; set; }

        [Required]
        public long RecruitPlanId { get; set; }

        [Required]
        public byte Result { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public long? EmployeeId { get; set; }

        public RecruitResult ToObject()
        {
            return new RecruitResult
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                EmployeeId = EmployeeId,
                RecruitPlanId = RecruitPlanId,
                ApplicantId = ApplicantId,
                Result = Result,
                RecruitResultId = RecruitResultId
            };
        }
    }
}