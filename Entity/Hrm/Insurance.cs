using System;

namespace Entity.Hrm
{
    public class Insurance
    {
        public long InsuranceId { get; set; }
        public long EmployeeId { get; set; }
        public string InsuranceNumber { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public int CityId { get; set; }
        public int? MonthBefore { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
    }
}