using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class SuggestJobConfigModel
    {
        [Required]
        public int JobConfigId { get; set; }

        [StringLength(50)]
        [Required]
        public string JobType { get; set; }

        [Required]
        public decimal JobPointMin { get; set; }

        [Required]
        public decimal JobPointMax { get; set; }

        public decimal? JobConditionMin { get; set; }
        public decimal? JobConditionMax { get; set; }

        public SuggetsJobConfig ToObject()
        {
            return new SuggetsJobConfig
            {
                JobConfigId = JobConfigId,
                JobType = JobType,
                JobPointMin = JobPointMin,
                JobPointMax = JobPointMax,
                JobConditionMin = JobConditionMin,
                JobConditionMax = JobConditionMax
            };
        }
    }
}