using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class DepartmentModel
    {
        [Required]
        public long DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(255)]
        public string DepartmentName { get; set; }

        public long? ParentId { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string Path { get; set; }

        public Department ToObject()
        {
            return new Department
            {
                Description = Description,
                IsActive = IsActive,
                DepartmentCode = DepartmentCode,
                DepartmentId = DepartmentId,
                DepartmentName = DepartmentName,
                ParentId = ParentId,
                Path = Path
            };
        }
    }
}