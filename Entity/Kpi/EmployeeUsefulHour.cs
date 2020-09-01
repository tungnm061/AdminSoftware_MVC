namespace Entity.Kpi
{
    public class EmployeeUsefulHour
    {
        public string EmployeeCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalFinish { get; set; }
        public decimal ToTalUsefulHoursReal { get; set; }
        public decimal ToTalUsefulHoursTask { get; set; }
        public string FullName { get; set; }
    }
}