using System;

namespace Entity.Kpi
{
    public class Complain
    {
        public string ComplainId { get; set; }
        public int CreateBy { get; set; }
        public int AccusedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int? ConfirmedBy { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public byte Status { get; set; }
        public string ConfirmedByName { get; set; }
        public string CreateByName { get; set; }
        public string AccusedByName { get; set; }
        public string AccusedByCode { get; set; }
        public string DepartmentName { get; set; }
        public string CreateByCode { get; set; }
    }
}