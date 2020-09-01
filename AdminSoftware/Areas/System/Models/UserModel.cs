using System;
using System.ComponentModel.DataAnnotations;
using Entity.System;

namespace AdminSoftware.Areas.System.Models
{
    public class UserModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int? UserGroupId { get; set; }

        [Required]
        public int? ModuleGroupId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public long? EmployeeId { get; set; }

        public User ToObject()
        {
            return new User
            {
                CreateDate = CreateDate,
                Email = Email,
                EmployeeId = EmployeeId,
                FullName = FullName,
                IsActive = IsActive,
                ModuleGroupId = ModuleGroupId,
                Password = Password,
                PhoneNumber = PhoneNumber,
                RoleId = RoleId,
                UserGroupId = UserGroupId,
                UserId = UserId,
                UserName = UserName
            };
        }
    }
}