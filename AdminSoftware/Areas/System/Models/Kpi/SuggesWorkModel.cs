using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class SuggesWorkModel
    {
        [Required]
        [StringLength(50)]
        public string SuggesWorkId { get; set; }
        [StringLength(50)]
        public string TaskId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public int CreateBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public string WorkPointType { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string Description { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public byte CalcType { get; set; }
        public decimal UsefulHoursTask { get; set; }
        public int WorkPointConfigId { get; set; }
        public long DepartmentId { get; set; }
        public string DescriptionTask { get; set; }
        public decimal UsefulHours { get; set; }
        public decimal? WorkPoint { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }

        public SuggesWork ToObject()
        {
            return new SuggesWork
            {
                SuggesWorkId = SuggesWorkId,
                TaskId = TaskId,
                FromDate = FromDate,
                ToDate = ToDate,
                Status = Status,
                CreateBy = CreateBy,
                CreateDate = CreateDate,
                VerifiedBy = VerifiedBy,
                VerifiedDate = VerifiedDate,
                ApprovedFisnishBy = ApprovedFisnishBy,
                ApprovedFisnishDate = ApprovedFisnishDate,
                FisnishDate = FisnishDate,
                Description = Description,
                UsefulHours = UsefulHours,
                TaskCode = TaskCode,
                TaskName = TaskName,
                CalcType = CalcType,
                UsefulHoursTask = UsefulHoursTask,
                WorkPointConfigId = WorkPointConfigId,
                DepartmentId = DepartmentId,
                DescriptionTask = DescriptionTask,
                Explanation = Explanation,
                WorkingNote = WorkingNote,
                WorkPointType = WorkPointType,
                WorkPoint= WorkPoint,
                DepartmentFisnishBy = DepartmentFisnishBy,
                DepartmentFisnishDate = DepartmentFisnishDate,
                Quantity = Quantity,
                FileConfirm = FileConfirm
            };
        }
    }
}