using System;

namespace Entity.Kpi
{
    public class WorkStreamDetail
    {
        public string WorkStreamDetailId { get; set; }
        public string TaskId { get; set; }
        public decimal? WorkPoint { get; set; }
        public string WorkStreamId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public decimal? UsefulHours { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public string WorkPointType { get; set; }
        public int WorkPointConfigId { get; set; }
        public int Quantity { get; set; }
        public string TaskName { get; set; }
        public string TaskCode { get; set; }
        public string CreateByName { get; set; }
        public string CreateByCode { get; set; }
        public string WorkStreamCode { get; set; }
        public string DepartmentName { get; set; }
        public string VerifiedByName { get; set; }
        public string FileConfirm { get; set; }
    }
}