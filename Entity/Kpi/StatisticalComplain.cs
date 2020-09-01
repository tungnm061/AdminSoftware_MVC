using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Kpi
{
    public class StatisticalComplain
    {
        public string EmployeeCode { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DepartmentName { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalFinish { get; set; }
        public string Rating { get; set; }
        public string FullName { get; set; }
        public string RatingPoint { get; set; }

    }
}
