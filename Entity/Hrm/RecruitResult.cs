using System;
using System.Collections.Generic;

namespace Entity.Hrm
{
    public class RecruitResult
    {
        public string RecruitResultId { get; set; }
        public string ApplicantId { get; set; }
        public long RecruitPlanId { get; set; }
        public byte Result { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public long? EmployeeId { get; set; }
        public string FullName { get; set; }
        public byte Sex { get; set; }
        public string RecruitPlanCode { get; set; }
        public int DepartmentId { get; set; }
        public string PositionId { get; set; }
        public List<RecruitResultDetail> RecruitResultDetails { get; set; }
    }
}