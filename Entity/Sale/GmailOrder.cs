using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class GmailOrder
    {
    public string GmailOrderId { get; set; }

    public int GmailId { get; set; }

    public string LinkOrder { get; set; }

    public DateTime OrderDate { get; set; }

    public int? CancelOrder { get; set; }

    public int? RefundOrder { get; set; }

    public string Description { get; set; }

    public DateTime CreateDate { get; set; }

    public int CreateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdateBy { get; set; }

}
    public class GmailOrderModel
    {
        public string GmailOrderId { get; set; }

        public int GmailId { get; set; }

        public string LinkOrder { get; set; }

        public DateTime OrderDate { get; set; }

        public int? CancelOrder { get; set; }

        public int? RefundOrder { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public int TotalOrder { get; set; }

        public List<GmailOrderDetail> GmailOrderDetails { get; set; }
    }

}
