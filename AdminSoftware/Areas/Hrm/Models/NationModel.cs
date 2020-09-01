using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class NationModel
    {
        [Required]
        public int NationId { get; set; }

        [Required]
        [StringLength(255)]
        public string NationName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Nation ToObject()
        {
            return new Nation
            {
                Description = Description,
                NationId = NationId,
                NationName = NationName
            };
        }
    }
}