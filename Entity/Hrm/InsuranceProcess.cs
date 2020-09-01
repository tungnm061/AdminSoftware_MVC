using System;

namespace Entity.Hrm
{
    public class InsuranceProcess
    {
        public string InsuranceProcessId { get; set; }
        public long InsuranceId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string InsuranceNumber { get; set; }
    }
}