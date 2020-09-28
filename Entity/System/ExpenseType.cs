using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.System
{
    public class ExpenseType
    {
        public int ExpenseId { get; set; }
        [LocalizeRequired]
        [DisplayName("Tên giao dịch")]
        [LocalizeStringLength(500)]
        public string ExpenseName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
