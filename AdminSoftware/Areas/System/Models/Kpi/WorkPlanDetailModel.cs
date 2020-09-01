using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class WorkPlanDetailModel
    {
        [Required]
        [StringLength(50)]
        public string WorkPlanDetailId { get; set; }

        [Required]
        [StringLength(50)]
        public string WorkPlanId { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public int Status { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public decimal? WorkPoint { get; set; }
        public string Explanation { get; set; }
        public string AssignWorkId { get; set; }
        public decimal? UsefulHours { get; set; }
        public  string WorkingNote { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string WorkPointType { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }

        public WorkPlanDetail ToObject()
        {
            return new WorkPlanDetail
            {
                WorkPlanDetailId = WorkPlanDetailId,
                WorkPlanId = WorkPlanId,
                TaskId = TaskId,
                FromDate = FromDate,
                ToDate = ToDate,
                Status = Status,
                Description = Description,
                Explanation = Explanation,
                UsefulHours= UsefulHours,
                WorkingNote = WorkingNote,
                ApprovedFisnishBy = ApprovedFisnishBy,
                ApprovedFisnishDate = ApprovedFisnishDate,
                FisnishDate = FisnishDate,
                WorkPointType = WorkPointType,
                WorkPoint = WorkPoint,
                DepartmentFisnishBy = DepartmentFisnishBy,
                DepartmentFisnishDate = DepartmentFisnishDate,
                Quantity = Quantity,
                FileConfirm = FileConfirm
            };
        }
    }
}