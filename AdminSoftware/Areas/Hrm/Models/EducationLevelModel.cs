using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class EducationLevelModel
    {
        [Required]
        public int EducationLevelId { get; set; }

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

        public EducationLevel ToObject()
        {
            return new EducationLevel
            {
                Description = Description,
                EducationLevelId = EducationLevelId,
                LevelName = LevelName,
                LevelCode = LevelCode
            };
        }
    }
}