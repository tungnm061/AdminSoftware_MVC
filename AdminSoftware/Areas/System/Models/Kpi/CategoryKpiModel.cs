using System;
using Entity.System;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class CategoryKpiModel
    {
        public int CategoryKpiId { get; set; }
        public string KpiCode { get; set; }
        public string KpiName { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        public CategoryKpi ToObject()
        {
            return new CategoryKpi
            {
                CategoryKpiId = CategoryKpiId,
                KpiCode = KpiCode,
                KpiName = KpiName,
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy
            };


        }
    }
}