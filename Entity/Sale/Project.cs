using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class Project
    {
        public string ProjectId { get; set; }

        public string ProjectCode { get; set; }

        public string FullName { get; set; }

        public int Status { get; set; }

        public string InvestorId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? ToDate { get; set; }

        public string Description { get; set; }
    }
}
