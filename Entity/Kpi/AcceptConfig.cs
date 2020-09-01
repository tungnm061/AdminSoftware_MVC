using System.Collections.Generic;

namespace Entity.Kpi
{
    public class AcceptConfig
    {
        public int AcceptConfigId { get; set; }
        public string AcceptType { get; set; }
        public decimal AcceptPointMin { get; set; }
        public decimal AcceptPointMax { get; set; }
        public decimal? AcceptConditionMin { get; set; }
        public decimal? AcceptConditionMax { get; set; }
    }

    public class TestReport
    {
        public List<TestChart> ListTestCharts { get; set; }
    }
    public class TestChart
    {
        public string Month { get; set; }
        public string Secstion { get; set; }
        public int Value { get; set; }
        public int Sex { get; set; }
    }
}