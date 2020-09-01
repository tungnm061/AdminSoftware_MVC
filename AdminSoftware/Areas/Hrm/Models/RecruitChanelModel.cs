using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class RecruitChanelModel
    {
        [Required]
        public int RecruitChanelId { get; set; }

        [Required]
        [StringLength(255)]
        public string ChanelName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public RecruitChanel ToObject()
        {
            return new RecruitChanel
            {
                Description = Description,
                IsActive = IsActive,
                ChanelName = ChanelName,
                RecruitChanelId = RecruitChanelId,
                CreateDate = CreateDate,
                CreateBy = CreateBy
            };
        }
    }
}