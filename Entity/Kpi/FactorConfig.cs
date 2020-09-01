namespace Entity.Kpi
{
    public class FactorConfig
    {
        public int FactorConfigId { get; set; }
        public string FactorType { get; set; }
        public decimal FactorPointMin { get; set; }
        public decimal FactorPointMax { get; set; }
        public decimal? FactorConditionMin { get; set; }
        public decimal? FactorConditionMax { get; set; }
    }
}