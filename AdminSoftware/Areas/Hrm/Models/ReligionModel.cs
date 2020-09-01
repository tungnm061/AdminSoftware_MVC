using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class ReligionModel
    {
        [Required]
        public int ReligionId { get; set; }

        [Required]
        [StringLength(255)]
        public string ReligionName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Religion ToObject()
        {
            return new Religion
            {
                Description = Description,
                ReligionId = ReligionId,
                ReligionName = ReligionName
            };
        }
    }
}