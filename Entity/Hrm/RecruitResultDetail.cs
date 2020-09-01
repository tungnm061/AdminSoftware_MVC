using System;

namespace Entity.Hrm
{
    public class RecruitResultDetail
    {
        public string RecruitResultId { get; set; }
        public long EmployeeId { get; set; }
        public byte Result { get; set; }
        public string Description { get; set; }
        public DateTime InterviewDate { get; set; }
        public string RecruitResultDetailId { get; set; }
    }
}