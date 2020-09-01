using System;

namespace Entity.Hrm
{
    public class Maternity
    {
        public string MaternityId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string RelaxStartTime { get; set; }
        public string RelaxEndTime { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string Description { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
    }
}