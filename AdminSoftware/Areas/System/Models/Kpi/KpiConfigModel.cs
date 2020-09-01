using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class KpiConfigModel
    {
        [Required]
        public int KpiConfigId { get; set; }

        [Required]
        public decimal MinHours { get; set; }

        [Required]
        public decimal MaxHours { get; set; }

        [Required]
        public byte PlanningDay { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanningHourMin { get; set; }
        [Required]
        [StringLength(50)]
        public string PlanningHourMax { get; set; }
        [Required]
        [StringLength(50)]
        public string HourConfirmMax { get; set; }
        [Required]
        [StringLength(50)]
        public string HourConfirmMin { get; set; }
        [Required]
        public decimal Notification { get; set; }

        public KpiConfig ToObject()
        {
            return new KpiConfig
            {
                KpiConfigId = KpiConfigId,
                MinHours = MinHours,
                MaxHours = MaxHours,
                PlanningDay = PlanningDay,
                PlanningHourMin = PlanningHourMin,
                PlanningHourMax = PlanningHourMax,
                HourConfirmMax = HourConfirmMax,
                HourConfirmMin = HourConfirmMin,
                Notification = Notification
            };
        }
    }
}