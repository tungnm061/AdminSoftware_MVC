using System;

namespace Entity.System
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int? UserGroupId { get; set; }
        public int? ModuleGroupId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public long? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public long? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string EmployeeName { get; set; }
        public int LocationEmployeeId { get; set; }
        public int DepartmentCompany { get; set; }
        public int? CategoryKpiId { get; set; }
        public string Path { get; set; }

    }
}