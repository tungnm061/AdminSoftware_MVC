using System;

namespace Entity.Kpi
{
    public class SuggesWork
    {
        public string SuggesWorkId { get; set; }
        public string TaskId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public byte Status { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public int? ApprovedFisnishBy { get; set; }
        public DateTime? ApprovedFisnishDate { get; set; }
        public DateTime? FisnishDate { get; set; }
        public int? DepartmentFisnishBy { get; set; }
        public DateTime? DepartmentFisnishDate { get; set; }
        public string Description { get; set; }
        public string Explanation { get; set; }
        public string WorkingNote { get; set; }
        public string TaskCode { get; set; }
        public string TaskName { get; set; }
        public byte CalcType { get; set; }
        public decimal UsefulHoursTask { get; set; }
        public int WorkPointConfigId { get; set; }
        public long DepartmentId { get; set; }
        public string DescriptionTask { get; set; }
        public string VerifiedByName { get; set; }
        public string CreateByName { get; set; }
        public string CreateByCode { get; set; }
        public decimal? UsefulHours { get; set; }
        public string WorkPointType { get; set; }
        public decimal? WorkPoint { get; set; }
        public int Quantity { get; set; }
        public string FileConfirm { get; set; }


    }
}