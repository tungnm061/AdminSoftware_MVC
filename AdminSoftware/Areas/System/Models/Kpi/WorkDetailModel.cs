using System;
using System.ComponentModel.DataAnnotations;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class WorkDetailModel
    {
        [Required]
        [StringLength(50)]
        public string WorkDetailId { get; set; }

        public string TaskId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        [Required]
        public int Status { get; set; }

        public decimal? UsefulHours { get; set; }

        [Required]
        public byte WorkType { get; set; }

        public string Description { get; set; }
        public int CreateBy { get; set; }
        public string WorkingNote { get; set; }
        public string Explanation { get; set; }
        public DateTime? FisnishDate { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public int? VerifiedBy { get; set; }
        public string WorkPointType { get; set; }
        public decimal WorkPoint { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }

    }
}