using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class FinishJobConfigModel
    {
        [Required]
        public int FinishConfigId { get; set; }

        [Required]
        [StringLength(50)]
        public string FinishType { get; set; }

        [Required]
        public decimal FinishPointMin { get; set; }

        [Required]
        public decimal FinishPointMax { get; set; }

        public decimal? FinishConditionMin { get; set; }
        public decimal? FinishConditionMax { get; set; }

        public FinishJobConfig ToObject()
        {
            return new FinishJobConfig
            {
                FinishConfigId = FinishConfigId,
                FinishType = FinishType,
                FinishPointMin = FinishPointMin,
                FinishPointMax = FinishPointMax,
                FinishConditionMin = FinishConditionMin,
                FinishConditionMax = FinishConditionMax
            };
        }
    }
}