using System;
using System.Collections.Generic;

namespace Entity.Sale
{
    public class Contract
    {
        public List<ContractDetail> ContractDetails;
        public long ContractId { get; set; }
        public string ContractCode { get; set; }
        public long CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string ContractNumber { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

    }
}