using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class ComplainModel
    {
        [Required]
        [StringLength(50)]
        public string ComplainId { get; set; }
        [Required]
        public int CreateBy { get; set; }
        [Required]
        public int AccusedBy { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        public int? ConfirmedBy { get; set; }

        public DateTime? ConfirmedDate { get; set; }
        [Required]
        public byte Status { get; set; }

        public Complain ToObject()
        {
            return new Complain
            {
               ComplainId = ComplainId,
                CreateBy = CreateBy,
                AccusedBy = AccusedBy,
                Description = Description,
                CreateDate = CreateDate,
                ConfirmedBy = ConfirmedBy,
                ConfirmedDate = ConfirmedDate,
                Status = Status
            };
        }
    }
}