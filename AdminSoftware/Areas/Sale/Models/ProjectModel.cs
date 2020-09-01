using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class ProjectModel
    {
        [Required]
        public string ProjectId { get; set; }
        [Required]
        public string ProjectCode { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string InvestorId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? ToDate { get; set; }

        public string Description { get; set; }

        public Project ToProject()
        {
            return new Project
            {
                ProjectId = ProjectId,
                ProjectCode = ProjectCode,
                FullName = FullName,
                Status = Status,
                InvestorId = InvestorId,
                FromDate = FromDate,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                ToDate = ToDate,
                Description = Description
            }
            ;
        }
    }
}