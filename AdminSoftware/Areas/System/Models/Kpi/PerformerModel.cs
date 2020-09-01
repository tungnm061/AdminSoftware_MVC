using System;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class PerformerModel
    {
        public int PerformerBy { get; set; }
        public string WorkStreamId { get; set; }


        public Performer ToObject()
        {
            return new Performer
            {
                PerformerBy = PerformerBy,
                WorkStreamId = WorkStreamId
      
            };
        }
    }
}