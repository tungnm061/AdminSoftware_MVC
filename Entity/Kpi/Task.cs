using System;

namespace Entity.Kpi
{
    public class Task
    {
        public string TaskId { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public byte CalcType { get; set; }
        public int WorkPointConfigId { get; set; }
        public decimal UsefulHours { get; set; }
        public bool Frequent { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string GroupName { get; set; }
        public int? CategoryKpiId { get; set; }
    }
}