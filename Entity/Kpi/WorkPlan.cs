using System;
using System.Collections.Generic;

namespace Entity.Kpi
{
    public class WorkPlan
    {
        public string WorkPlanId { get; set; }
        public string WorkPlanCode { get; set; }
        public int CreateBy { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public List<WorkPlanDetail> WorkPlanDetails { get; set; }
        public int? ConfirmedBy { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ConfirmedByName { get; set; }
        public string ApprovedByName { get; set; }
        public string CreateByName { get; set; }
        public long DepartmentId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public int NeedCompleted { get; set; }
        public string CreateDateFormat => CreateDate.ToLongTimeString()+ "-"+ CreateDate.DayOfWeek + "-"+ CreateDate.ToShortDateString();
    }
}