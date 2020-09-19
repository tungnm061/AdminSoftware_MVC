using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
    public class ExpenseType
    {
        public int ExpenseId { get; set; }

        public string ExpenseName { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}
