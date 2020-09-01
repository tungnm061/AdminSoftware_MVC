using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class TrainingLevelModel
    {
        [Required]
        public int TrainingLevelId { get; set; }

        [Required]
        [StringLength(255)]
        public string LevelName { get; set; }

        [Required]
        [StringLength(50)]
        public string LevelCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public TrainingLevel ToObject()
        {
            return new TrainingLevel
            {
                Description = Description,
                TrainingLevelId = TrainingLevelId,
                LevelName = LevelName,
                LevelCode = LevelCode
            };
        }
    }
}