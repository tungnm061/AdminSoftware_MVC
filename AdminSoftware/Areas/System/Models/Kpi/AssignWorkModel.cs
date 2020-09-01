using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class AssignWorkModel
    {
        [Required]
        [StringLength(50)]
        public string AssignWorkId { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskId { get; set; }

        [Required]
        public int CreateBy { get; set; }

        public int AssignBy { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public decimal UsefulHours { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string WorkPointType { get; set; }
        public decimal? WorkPoint { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }
        public int? DepartmentFollowBy { get; set; }
        public int? DirectorFollowBy { get; set; }
        public  int? TypeAssignWork { get; set; }
        public AssignWork ToObject()
        {
            return new AssignWork
            {
                AssignWorkId = AssignWorkId,
                TaskId = TaskId,
                AssignBy = AssignBy,
                CreateDate = CreateDate,
                FromDate = FromDate,
                Status = Status,
                ToDate = ToDate,
                Description = Description,
                CreateBy = CreateBy,
                UsefulHours= UsefulHours,
                Explanation = Explanation,
                WorkingNote = WorkingNote,
                ApprovedFisnishBy = ApprovedFisnishBy,
                ApprovedFisnishDate = ApprovedFisnishDate,
                FisnishDate = FisnishDate,
                WorkPointType = WorkPointType,
                WorkPoint = WorkPoint,
                DepartmentFisnishBy = DepartmentFisnishBy,
                DepartmentFisnishDate = DepartmentFisnishDate,
                Quantity = Quantity,
                FileConfirm = FileConfirm,
                DepartmentFollowBy = DepartmentFollowBy,
                DirectorFollowBy = DirectorFollowBy,
                TypeAssignWork = TypeAssignWork
            };
        }
    }
}