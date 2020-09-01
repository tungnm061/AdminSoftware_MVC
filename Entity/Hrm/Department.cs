namespace Entity.Hrm
{
    public class Department
    {
        public long DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Path { get; set; }
        public int EmployeeOns { get; set; }
        public int EmployeeOffs { get; set; }
    }
}