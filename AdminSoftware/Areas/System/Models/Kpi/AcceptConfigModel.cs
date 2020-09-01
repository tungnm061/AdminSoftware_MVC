using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class AcceptConfigModel
    {
        [Required]
        public int AcceptConfigId { get; set; }

        [Required]
        [StringLength(50)]
        public string AcceptType { get; set; }

        [Required]
        public decimal AcceptPointMin { get; set; }

        [Required]
        public decimal AcceptPointMax { get; set; }

        public decimal? AcceptConditionMin { get; set; }
        public decimal? AcceptConditionMax { get; set; }

        public AcceptConfig ToObject()
        {
            return new AcceptConfig
            {
                AcceptConfigId = AcceptConfigId,
                AcceptType = AcceptType,
                AcceptPointMin = AcceptPointMin,
                AcceptPointMax = AcceptPointMax,
                AcceptConditionMin = AcceptConditionMin,
                AcceptConditionMax = AcceptConditionMax
            };
        }
    }
}