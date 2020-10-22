using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class Listting
    {
        public int ListtingId { get; set; }
        [LocalizeRequired]
        [DisplayName("Tài khoản gmail")]
        public int GmailId { get; set; }
        [LocalizeRequired]
        [DisplayName("Ba số PayOnner")]
        public string ThreeNumberPayOnner { get; set; }
        [LocalizeRequired]
        [DisplayName("PayOnner")]
        //[MinLengthAttribute(10)]
        public string PayOnner { get; set; }
        [LocalizeRequired]
        [DisplayName("Listting")]
        public int? ListProduct { get; set; }
        [LocalizeRequired]
        [DisplayName("Balance")]
        public decimal? Balance { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public string Description { get; set; }

    }
}
