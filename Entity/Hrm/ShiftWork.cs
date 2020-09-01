using System;

namespace Entity.Hrm
{
    public class ShiftWork
    {
        public int ShiftWorkId { get; set; }
        public string ShiftWorkCode { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RelaxStartTime { get; set; }
        public string RelaxEndTime { get; set; }
        public string Description { get; set; }
        public decimal ToTalTimeHours
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime) || string.IsNullOrEmpty(RelaxStartTime) ||
                    string.IsNullOrEmpty(EndTime) || string.IsNullOrEmpty(RelaxEndTime))
                    return 0;
                return
                    (decimal)(
                        (DateTime.Parse(DateTime.Now.ToShortDateString() + " " + RelaxStartTime) -
                         DateTime.Parse(DateTime.Now.ToShortDateString() + " " + StartTime))
                        + (DateTime.Parse(DateTime.Now.ToShortDateString() + " " + EndTime) -
                           DateTime.Parse(DateTime.Now.ToShortDateString() + " " + RelaxEndTime))
                        ).TotalHours;
            }
        }
    }
}