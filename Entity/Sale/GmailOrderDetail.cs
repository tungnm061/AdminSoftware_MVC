using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class GmailOrderDetail
    {
        public string GmailOrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public int? TotalOrder { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

    }
}
