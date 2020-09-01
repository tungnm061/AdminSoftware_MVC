using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class PositionModel
    {
        [Required]
        public int PositionId { get; set; }

        [Required]
        [StringLength(255)]
        public string PositionName { get; set; }

        [Required]
        [StringLength(50)]
        public string PositionCode { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Position ToObject()
        {
            return new Position
            {
                Description = Description,
                PositionId = PositionId,
                PositionName = PositionName,
                PositionCode = PositionCode
            };
        }
    }
}