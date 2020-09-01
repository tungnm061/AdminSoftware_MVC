using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class WorkStreamDetailModel
    {
        [Required]
        [StringLength(50)]
        public string WorkStreamDetailId { get; set; }
        [Required]
        [StringLength(50)]
        public string TaskId { get; set; }
        [Required]
        [StringLength(50)]
        public string WorkStreamId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public int CreateBy { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public int Status { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsDefault { get; set; }
        public decimal? UsefulHours { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public string CreateByName { get; set; }
        public string CreateByCode{ get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public string WorkPointType { get; set; }
        public decimal? WorkPoint { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }

        public WorkStreamDetail ToObject()
        {
            return new WorkStreamDetail
            {
                WorkStreamDetailId = WorkStreamDetailId,
                TaskId = TaskId,
                WorkStreamId = WorkStreamId,
                FromDate = FromDate,
                ToDate = ToDate,
                CreateBy = CreateBy,
                CreateDate = CreateDate,
                Status = Status,
                Description = Description,
                IsDefault = IsDefault,
                UsefulHours = UsefulHours,
                VerifiedBy = VerifiedBy,
                VerifiedDate = VerifiedDate,
                ApprovedFisnishBy = ApprovedFisnishBy,
                ApprovedFisnishDate = ApprovedFisnishDate,
                FisnishDate = FisnishDate,
                Explanation = Explanation,
                WorkingNote = WorkingNote,
                WorkPointType = WorkPointType,
                WorkPoint = WorkPoint,
                DepartmentFisnishBy = DepartmentFisnishBy,
                DepartmentFisnishDate = DepartmentFisnishDate,
                Quantity = Quantity,
                CreateByCode = CreateByCode,
                FileConfirm = FileConfirm
            };
        }
    }
}