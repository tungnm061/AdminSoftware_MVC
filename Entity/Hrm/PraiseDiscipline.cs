using System;
using System.Collections.Generic;

namespace Entity.Hrm
{
    public class PraiseDiscipline
    {
        public string PraiseDisciplineId { get; set; }
        public string PraiseDisciplineCode { get; set; }
        public byte PraiseDisciplineType { get; set; }
        public string Title { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime PraiseDisciplineDate { get; set; }
        public string Formality { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public List<PraiseDisciplineDetail> PraiseDisciplineDetails { get; set; }
    }
}