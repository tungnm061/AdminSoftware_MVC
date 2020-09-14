using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
     public class Listting
    {
        public int ListtingId { get; set; }

        public int GmailId { get; set; }

        public int? ThreeNumberPayOnner { get; set; }

        public int? PayOnner { get; set; }

        public int? ListProduct { get; set; }

        public decimal? Balance { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public string Description { get; set; }

    }
}
