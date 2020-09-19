using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
   public class OrderStatistical
    {
        public int GmailId { get; set; }
        public string LinkUrl { get; set; }

        public List<StatisticalDetail> StatisticalDetails { get; set; }

        public  decimal TotalOrderPrice { get; set; }
        public decimal TotalFinishPrice { get; set; }
        public decimal FinishOrderPrice { get; set; }
        public decimal CancelOrderPrice { get; set; }
        public decimal TotalFinish { get; set; }
        public decimal TotalCancel { get; set; }
        public decimal TotalOrder { get; set; }

    }

    public class StatisticalDetail
   {
       public DateTime DateDay { get; set; }
       public int TotalOrder { get; set; }
       public int FinishOrder { get; set; }
       public int CancelOrder { get; set; }

    }
}
