using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
    public class CompanyBank
    {
        public  long CompanyBankId { get; set; }
        public  int ExpenseType { get; set; }

        public byte TypeMonney { get; set; }

        public decimal MoneyNumber { get; set; } 

        public DateTime TradingDate { get; set; }

        public int TradingBy { get; set; }

        public string ExpenseText { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
