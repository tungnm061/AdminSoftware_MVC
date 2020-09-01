using System;
using System.Collections.Generic;

namespace Entity.Kpi
{
    public class WorkStream
    {
        public string WorkStreamId { get; set; }
        public List<WorkStreamDetail> WorkStreamDetails { get; set; }
        public List<Performer> Performers { get; set; }
        public string WorkStreamCode { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string AssignWorkId { get; set; }
        public string TaskId { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public List<string> PerformerBys { get; set; }
        public string TaskCode { get; set; }
        public string TaskAssCode { get; set; }
        public string ApprovedByName { get; set; }
        public string TaskName { get; set; }
        public string TaskAssName { get; set; }
        public string CreateByName { get; set; }
        public string TaskNameFull => TaskName + TaskAssName;
        public string TaskCodeFull => TaskCode + TaskAssCode;
    }
}