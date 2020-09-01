namespace Entity.Kpi
{
    public class KpiConfig
    {
        public int KpiConfigId { get; set; }
        public decimal MinHours { get; set; }
        public decimal MaxHours { get; set; }
        public byte PlanningDay { get; set; }
        public string PlanningHourMax { get; set; }
        public string PlanningHourMin { get; set; }
        public string HourConfirmMax { get; set; }
        public string HourConfirmMin { get; set; }
        public decimal Notification { get; set; }
    }
}