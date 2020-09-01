using System;

namespace Entity.Hrm
{
    public class JobChange
    {
        public string JobChangeId { get; set; }
        public string JobChangeCode { get; set; }
        public long EmployeeId { get; set; }
        public long FromDepartmentId { get; set; }
        public long ToDepartmentId { get; set; }
        public int? FromPositionId { get; set; }
        public int? ToPositionId { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string JobChangeFile { get; set; }
        public string JobChangeNumber { get; set; }
    }
}