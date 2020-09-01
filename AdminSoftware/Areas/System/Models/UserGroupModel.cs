using System.ComponentModel.DataAnnotations;
using Entity.System;

namespace AdminSoftware.Areas.System.Models
{
    public class UserGroupModel
    {
        [Required]
        public int UserGroupId { get; set; }

        [Required]
        [StringLength(50)]
        public string GroupCode { get; set; }

        [Required]
        [StringLength(255)]
        public string GroupName { get; set; }

        public string Description { get; set; }

        public UserGroup ToObject()
        {
            return new UserGroup
            {
                UserGroupId = UserGroupId,
                GroupName = GroupName,
                Description = Description,
                GroupCode = GroupCode
            };
        }
    }
}