using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class PoPayment
    {
        [LocalizeRequired]
        public long PoPaymentId { get; set; }

        [LocalizeRequired]
        [DisplayName("Số tiền")]
        public decimal MoneyNumber { get; set; }

        [LocalizeRequired]
        [DisplayName("Tỷ giá")]
        public decimal RateMoney { get; set; }

        public byte TypeMoney { get; set; }

        [LocalizeRequired]
        [DisplayName("Ngày giao dịch")]
        public DateTime TradingDate { get; set; }

        [LocalizeRequired]
        [DisplayName("Trạng thái")]
        public byte Status { get; set; }

        public int? ConfirmBy { get; set; }

        public DateTime? ConfirmDate { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        [LocalizeRequired]
        [DisplayName("Người giao dịch")]
        public int TradingBy { get; set; }

        public string Path { get; set; }

        public string TradingMonth { get; set; }
    }
}
