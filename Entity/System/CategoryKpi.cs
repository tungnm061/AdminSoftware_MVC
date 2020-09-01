using System;

namespace Entity.System
{
    public class CategoryKpi
    {
        public int CategoryKpiId { get; set; }
        public string KpiCode { get; set; }
        public string KpiName { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
    }
}