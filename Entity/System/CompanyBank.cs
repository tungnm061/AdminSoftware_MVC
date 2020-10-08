using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.System
{
    public class CompanyBank
    {
        public  long CompanyBankId { get; set; }
        [LocalizeRequired]
        [DisplayName("Loại giao dịch")]
        public  int? ExpenseId { get; set; }
        [LocalizeRequired]
        [DisplayName("Tiền tệ")]
        public byte TypeMonney { get; set; }

        [LocalizeRequired]
        [DisplayName("Số tiền")]
        public decimal MoneyNumber { get; set; }
        [LocalizeRequired]
        [DisplayName("Ngày giao dịch")]
        public DateTime TradingDate { get; set; }
        [LocalizeRequired]
        [DisplayName("Người giao dịch")]
        public int TradingBy { get; set; }
        [LocalizeStringLength(500)]
        public string ExpenseText { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
        
        public  string TradingMonth { get; set; }

        public  string FilePath { get; set; }

        public  string TextNote { get; set; }

        public int ConfirmBy { get; set; }

        public DateTime? ConfirmDate { get; set; }
        [LocalizeRequired]
        [DisplayName("Trạng thái")]
        public byte? Status { get; set; }

        public decimal MoneyNumberVND
        {
            get
            {
                if (TypeMonney == 1)
                {
                    return 0;
                }
                return MoneyNumber;
            }
        }

        public decimal MoneyNumberUSD
        {
            get
            {
                if (TypeMonney != 1)
                {
                    return 0;
                }
                return MoneyNumber;
            }
        }
    }
}
