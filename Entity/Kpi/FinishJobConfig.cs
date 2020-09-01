namespace Entity.Kpi
{
    public class FinishJobConfig
    {
        public int FinishConfigId { get; set; }
        public string FinishType { get; set; }
        public decimal FinishPointMin { get; set; }
        public decimal FinishPointMax { get; set; }
        public decimal? FinishConditionMin { get; set; }
        public decimal? FinishConditionMax { get; set; }
    }
}