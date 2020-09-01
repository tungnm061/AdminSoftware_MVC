using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class MedicalModel
    {
        [Required]
        public int MedicalId { get; set; }

        [Required]
        [StringLength(255)]
        public string MedicalName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Medical ToObject()
        {
            return new Medical
            {
                Description = Description,
                MedicalId = MedicalId,
                MedicalName = MedicalName
            };
        }
    }
}