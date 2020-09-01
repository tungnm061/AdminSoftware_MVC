using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class PraiseDisciplineModel
    {
        [Required]
        public string PraiseDisciplineId { get; set; }

        [Required]
        [StringLength(50)]
        public string PraiseDisciplineCode { get; set; }

        [Required]
        public byte PraiseDisciplineType { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string DecisionNumber { get; set; }

        [Required]
        public DateTime PraiseDisciplineDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Formality { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public PraiseDiscipline ToObject()
        {
            return new PraiseDiscipline
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                Title = Title,
                Reason = Reason,
                PraiseDisciplineDate = PraiseDisciplineDate,
                DecisionNumber = DecisionNumber,
                Formality = Formality,
                PraiseDisciplineCode = PraiseDisciplineCode,
                PraiseDisciplineId = PraiseDisciplineId,
                PraiseDisciplineType = PraiseDisciplineType
            };
        }
    }
}