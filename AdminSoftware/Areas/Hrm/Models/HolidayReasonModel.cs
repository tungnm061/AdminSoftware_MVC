using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class HolidayReasonModel
    {
        [Required]
        public int HolidayReasonId { get; set; }

        [Required]
        [StringLength(50)]
        public string ReasonCode { get; set; }

        [Required]
        [StringLength(255)]
        public string ReasonName { get; set; }


        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public decimal PercentSalary { get; set; }
        public HolidayReason ToObject()
        {
            return new HolidayReason
            {
                Description = Description,
                IsActive = IsActive,
                HolidayReasonId = HolidayReasonId,
                ReasonCode = ReasonCode,
                ReasonName = ReasonName,
                PercentSalary = PercentSalary
            };
        }
    }
}