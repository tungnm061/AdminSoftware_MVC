using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class SchoolModel
    {
        [Required]
        public int SchoolId { get; set; }

        [Required]
        [StringLength(255)]
        public string SchoolName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public School ToObject()
        {
            return new School
            {
                Description = Description,
                SchoolId = SchoolId,
                SchoolName = SchoolName
            };
        }
    }
}