using System;

namespace Entity.Hrm
{
    public class RecruitChanel
    {
        public int RecruitChanelId { get; set; }
        public string ChanelName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}