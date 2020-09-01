using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class ShiftWorkModel
    {
        [Required]
        public int ShiftWorkId { get; set; }

        [Required]
        [StringLength(50)]
        public string ShiftWorkCode { get; set; }

        [Required]
        [StringLength(50)]
        public string StartTime { get; set; }

        [Required]
        [StringLength(50)]
        public string EndTime { get; set; }

        public string RelaxStartTime { get; set; }
        public string RelaxEndTime { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public ShiftWork ToObject()
        {
            return new ShiftWork
            {
                Description = Description,
                EndTime = EndTime,
                RelaxEndTime = RelaxEndTime,
                RelaxStartTime = RelaxStartTime,
                ShiftWorkCode = ShiftWorkCode,
                ShiftWorkId = ShiftWorkId,
                StartTime = StartTime
            };
        }
    }
}