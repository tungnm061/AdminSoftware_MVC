using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Hrm
{
    public class CheckTimeSheet
    {
        public DateTime TimeSheetDate { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
    }
}
