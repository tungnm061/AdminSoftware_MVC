using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class ContractTypeModel
    {
        [Required]
        public int ContractTypeId { get; set; }

        [Required]
        [StringLength(255)]
        public string TypeName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ContractType ToObject()
        {
            return new ContractType
            {
                Description = Description,
                IsActive = IsActive,
                ContractTypeId = ContractTypeId,
                TypeName = TypeName
            };
        }
    }
}