using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
   public class Gmail
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public int CreateUser { get; set; }
        public int RemoveUser { get; set; }
        public int ListtingUser { get; set; }
        public int OrderUser { get; set; }

    }
}
