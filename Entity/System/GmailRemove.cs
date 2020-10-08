using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.System
{
    public class GmailRemove
    {
        
        public int GmailRemoveId { get; set; }
        [Required]
        [DisplayName("Gmail gỡ tài khoản")]
        public int GmailId { get; set; }
        [Required]
        [DisplayName("Mật khẩu gmail Reg tài khoản")]
        public string Password { get; set; }

        public int? GmailRestoreId { get; set; }

        public int? GmailChangeId { get; set; }

        public string PasswordGmailChange { get; set; }

        public int? GmailRestoreChangeId { get; set; }

        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
