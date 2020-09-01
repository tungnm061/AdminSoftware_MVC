namespace Entity.Kpi
{
    public class SuggetsJobConfig
    {
        public int JobConfigId { get; set; }
        public string JobType { get; set; }
        public decimal JobPointMin { get; set; }
        public decimal JobPointMax { get; set; }
        public decimal? JobConditionMin { get; set; }
        public decimal? JobConditionMax { get; set; }
    }
}