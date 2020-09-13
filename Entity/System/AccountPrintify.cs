using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
    public class AccountPrintify
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }

    }
}
