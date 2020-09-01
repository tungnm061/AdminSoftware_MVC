namespace Entity.Hrm
{
    public class HolidayReason
    {
        public int HolidayReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public decimal PercentSalary { get; set; }
    }
}