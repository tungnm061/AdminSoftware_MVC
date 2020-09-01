using System;

namespace Entity.Hrm
{
    public class Contract
    {
        public string ContractId { get; set; }
        public string ContractCode { get; set; }
        public long EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ContractTypeId { get; set; }
        public string ContractFile { get; set; }
        public string ContractOthorFile { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}