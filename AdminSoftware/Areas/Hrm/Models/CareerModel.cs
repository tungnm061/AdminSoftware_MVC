using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class CareerModel
    {
        [Required]
        public int CareerId { get; set; }

        [Required]
        [StringLength(255)]
        public string CareerName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Career ToObject()
        {
            return new Career
            {
                Description = Description,
                CareerId = CareerId,
                CareerName = CareerName
            };
        }
    }
}