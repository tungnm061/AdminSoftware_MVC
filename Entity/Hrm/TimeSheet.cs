using System;

namespace Entity.Hrm
{
    public class TimeSheet
    {
        public string TimeSheetId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime TimeSheetDate { get; set; }
        public DateTime? Checkin { get; set; }
        public DateTime? Checkout { get; set; }
        public string StartTime { get; set; }
        public string RelaxStartTime { get; set; }
        public string EndTime { get; set; }
        public string RelaxEndTime { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public int ShiftWorkId { get; set; }
        public decimal RequirementTimeFirst
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(RelaxStartTime))
                    return 0;
                return
                    (decimal) (DateTime.Parse(DateTime.Now.ToShortDateString() + " " + RelaxStartTime) -
                               DateTime.Parse(DateTime.Now.ToShortDateString() + " " + StartTime)).TotalMinutes;
            }
        }

        public decimal RequirementTimeLast
        {
            get
            {
                if (string.IsNullOrEmpty(EndTime) || string.IsNullOrEmpty(RelaxEndTime))
                    return 0;
                return
                    (decimal) (DateTime.Parse(DateTime.Now.ToShortDateString() + " " + EndTime) -
                               DateTime.Parse(DateTime.Now.ToShortDateString() + " " + RelaxEndTime)).TotalMinutes;
            }
        }

        public decimal RealTimeFirst
        {
            get
            {
                if (Checkin == null || Checkout == null)
                    return 0;
                if (Checkin.Value >= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime))
                    return 0;
                if (Checkin.Value > DateTime.Parse(Checkin.Value.ToShortDateString() + " " + StartTime))
                {
                    if (Checkout != null &&
                        Checkout.Value <= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime))
                        return (decimal) (Checkout.Value - Checkin.Value).TotalMinutes;
                    return
                        (decimal) (DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime) -
                                   Checkin.Value).TotalMinutes;
                }
                if (Checkout != null &&
                    Checkout.Value <= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime))
                    return
                        (decimal)
                            (Checkout.Value - DateTime.Parse(Checkin.Value.ToShortDateString() + " " + StartTime))
                                .TotalMinutes;
                return
                    (decimal) (DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime) -
                               DateTime.Parse(Checkin.Value.ToShortDateString() + " " + StartTime)).TotalMinutes;
            }
        }

        public decimal RealTimeSecond
        {
            get
            {
                if (Checkin == null || Checkout == null)
                    return 0;
                if (Checkout.Value <= DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxEndTime))
                    return 0;
                if (Checkout.Value < DateTime.Parse(Checkout.Value.ToShortDateString() + " " + EndTime))
                {
                    if (Checkin != null &&
                        Checkin.Value >= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxEndTime))
                        return (decimal) (Checkout.Value - Checkin.Value).TotalMinutes;
                    return
                        (decimal)
                            (Checkout.Value - DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxEndTime))
                                .TotalMinutes;
                }
                if (Checkin != null &&
                    Checkin.Value >= DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxEndTime))
                    return
                        (decimal)
                            (DateTime.Parse(Checkout.Value.ToShortDateString() + " " + EndTime) - Checkin.Value)
                                .TotalMinutes;
                return
                    (decimal) (DateTime.Parse(Checkout.Value.ToShortDateString() + " " + EndTime) -
                               DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxEndTime)).TotalMinutes;
            }
        }

        public decimal RealMinutes => Math.Round((RealTimeFirst + RealTimeSecond), 0, MidpointRounding.AwayFromZero);
        public decimal RealHours => Math.Round((RealMinutes/60), 1, MidpointRounding.AwayFromZero);

        public decimal LateMinutes
        {
            get
            {
                if (Checkin == null || Checkout == null)
                    return 0;
                if (Checkin.Value >= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + EndTime))
                    return 0;
                if (Checkin.Value > DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxEndTime))
                    return Math.Round(RequirementTimeFirst +
                           (decimal)(Checkin.Value - DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxEndTime))
                               .TotalMinutes,0,MidpointRounding.AwayFromZero);
                if (Checkin.Value >= DateTime.Parse(Checkin.Value.ToShortDateString() + " " + RelaxStartTime))
                    return Math.Round(RequirementTimeFirst, 0, MidpointRounding.AwayFromZero);
                if (Checkin.Value > DateTime.Parse(Checkin.Value.ToShortDateString() + " " + StartTime))
                    return
                        (decimal)Math.Round(
                            (Checkin.Value - DateTime.Parse(Checkin.Value.ToShortDateString() + " " + StartTime))
                                .TotalMinutes, 0, MidpointRounding.AwayFromZero);
                return 0;
            }
        }

        public decimal EarlyMinutes
        {
            get
            {
                if (Checkin == null || Checkout == null)
                    return 0;
                if (Checkout.Value <= DateTime.Parse(Checkout.Value.ToShortDateString() + " " + StartTime))
                    return 0;
                if (Checkout.Value <= DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxStartTime))
                    return Math.Round(RequirementTimeLast +
                           (decimal)(DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxStartTime) - Checkout.Value)
                               .TotalMinutes, 0, MidpointRounding.AwayFromZero);
                if (Checkout.Value < DateTime.Parse(Checkout.Value.ToShortDateString() + " " + RelaxEndTime))
                    return Math.Round(RequirementTimeLast, 0, MidpointRounding.AwayFromZero);
                if (Checkout.Value < DateTime.Parse(Checkout.Value.ToShortDateString() + " " + EndTime))
                    return
                        (decimal)
                            Math.Round(
                                (DateTime.Parse(Checkout.Value.ToShortDateString() + " " + EndTime) -Checkout.Value)
                                    .TotalMinutes, 0, MidpointRounding.AwayFromZero);
                return 0;
            }
        }

        public decimal RealDays
            =>
                (RequirementTimeFirst + RequirementTimeLast) == 0
                    ? 0
                    : Math.Round(
                        (RealTimeFirst + RealTimeSecond)/(RequirementTimeFirst + RequirementTimeLast), 1,
                        MidpointRounding.AwayFromZero);
    }
}