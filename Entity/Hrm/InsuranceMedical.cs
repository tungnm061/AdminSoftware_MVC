using System;

namespace Entity.Hrm
{
    public class InsuranceMedical
    {
        public string InsuranceMedicalId { get; set; }
        public long EmployeeId { get; set; }
        public string InsuranceMedicalNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int CityId { get; set; }
        public int MedicalId { get; set; }
        public decimal Amount { get; set; }
        public string AmountString => "  " + Amount.ToString("n0");
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
    }
}