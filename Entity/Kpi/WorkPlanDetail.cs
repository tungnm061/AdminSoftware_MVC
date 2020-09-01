using System;

namespace Entity.Kpi
{
    public class WorkPlanDetail
    {
        public string WorkPlanDetailId { get; set; }
        public string WorkPlanId { get; set; }
        public string WorkPlanCode { get; set; }
        public int CreateBy { get; set; }
        public string TaskId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public string WorkPointType { get; set; }
        public int WorkPointConfigId { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public decimal UsefulHourTask { get; set; }
        public decimal? UsefulHours { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public string CreateByUserName { get; set; }
        public string CreateByFullName { get; set; }
        public string ConfirmByName { get; set; }
        public long DepartmentId { get; set; }
        public decimal? WorkPoint { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }
    }
}