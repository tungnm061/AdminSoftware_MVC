using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class PoPayment
    {
        public long PoPaymentId { get; set; }

        public decimal MoneyNumber { get; set; }

        public decimal? RateMoney { get; set; }

        public byte? TypeMoney { get; set; }

        public DateTime TradingDate { get; set; }

        public byte Status { get; set; }

        public int? ConfirmBy { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }
    }
}
