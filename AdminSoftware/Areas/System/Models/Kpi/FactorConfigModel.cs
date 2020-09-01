using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class FactorConfigModel
    {
        [Required]
        public int FactorConfigId { get; set; }

        [Required]
        [StringLength(50)]
        public string FactorType { get; set; }

        [Required]
        public decimal FactorPointMin { get; set; }

        [Required]
        public decimal FactorPointMax { get; set; }

        public decimal? FactorConditionMin { get; set; }
        public decimal? FactorConditionMax { get; set; }

        public FactorConfig ToObject()
        {
            return new FactorConfig
            {
                FactorConfigId = FactorConfigId,
                FactorType = FactorType,
                FactorPointMin = FactorPointMin,
                FactorPointMax = FactorPointMax,
                FactorConditionMin = FactorConditionMin,
                FactorConditionMax = FactorConditionMax

            };
        }
    }
}