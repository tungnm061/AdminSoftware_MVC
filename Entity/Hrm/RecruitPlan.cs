using System;
using System.Dynamic;

namespace Entity.Hrm
{
    public class RecruitPlan
    {
        public long RecruitPlanId { get; set; }
        public string RecruitPlanCode { get; set; }
        public string Title { get; set; }
        public long DepartmentId { get; set; }
        public int PositionId { get; set; }
        public int Quantity { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Requirements { get; set; }
        public string ChanelIds { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public int ApplicantNumber { get; set; }
        public int ApplicantPass { get; set; }

        public ExpandoObject DynamicObject
        {
            get
            {
                dynamic dynamicObject = new ExpandoObject();
                dynamicObject.Name = "Thi Ngô";
                dynamicObject.PhoneNumber = "0974784881";
                return dynamicObject;
            }
        }
    }
}