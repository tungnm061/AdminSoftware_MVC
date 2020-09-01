using System;
using System.Collections.Generic;

namespace Entity.Hrm
{
    public class EmployeeTimeSheet
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long DepartmentId { get; set; }
        public List<CheckTimeSheet> CheckTimeSheets { get; set; }
        public decimal? TotalPoint { get; set; }
        public decimal? TotalDayPoint { get; set; }
        public decimal? RealPoint { get; set; }
        public decimal? EarlyMinutes { get; set; }
        public int? EarlyNumber { get; set; }
        public int? LateNumber { get; set; }
        public decimal? LateMinutes { get; set; }
        public decimal ActualHour { get; set; }
        public decimal FixedHour { get; set; }
        public decimal HolidayPoints { get; set; }
        public decimal ActualToTalPoint { get; set; }
        public decimal ResponsibilitySalary { get; set; }
        public decimal ProfessionalSalary { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal IncurredSalary { get; set; }
        public decimal FactorPoint { get; set; }
        public decimal? TotalDayHoliday { get; set; }
        public decimal? DayPointOt { get; set; }
        public string DepartmentName { get; set; }
        public decimal TotalEarlyLateMinutes => (decimal) (LateMinutes + EarlyMinutes);

        public decimal TotalDaySalary
        {
            get
            {
                if (60 <= TotalEarlyLateMinutes && TotalEarlyLateMinutes < 240)
                {
                    return (TotalDayPoint ?? 0) - (decimal)1.5;
                }
                if (240 <= TotalEarlyLateMinutes && TotalEarlyLateMinutes < 360)
                {
                    return (TotalDayPoint ?? 0) - (decimal)2;
                }
                if (360 <= TotalEarlyLateMinutes && TotalEarlyLateMinutes < 480)
                {
                    return (TotalDayPoint ?? 0) - (decimal)3;
                }
                if (480 <= TotalEarlyLateMinutes)
                {
                    return (TotalDayPoint ?? 0) - (decimal)4;
                }
                if (TotalEarlyLateMinutes == 0)
                {
                    return (TotalDayPoint ?? 0);
                }
                return 0;
            }
        }
        public decimal? TotalDaySalary60Minutes { get; set; }
        public decimal FactorDay
        {
            get
            {
                if ( RealPoint == null || RealPoint == 0)
                    return 0;
                else
                {
                    return Math.Round((decimal)((TotalDaySalary + (TotalDaySalary60Minutes ?? 0)) / RealPoint),2,MidpointRounding.AwayFromZero);
                }
            }
        }
        public decimal TotalSalary => SalaryKpi + SalaryHoliday + SalaryOt;
        public decimal SalaryKpi
        {
            get
            {
                if (RealPoint == null || RealPoint == 0)
                    return 0;
                else
                {
                    return
                        (decimal)(((BasicSalary + ProfessionalSalary + ResponsibilitySalary) * FactorDay * FactorPoint) + IncurredSalary);
                }
            }
        }

        public decimal SalaryOt { get; set; }
        public decimal SalaryHoliday { get; set; }
    }
}