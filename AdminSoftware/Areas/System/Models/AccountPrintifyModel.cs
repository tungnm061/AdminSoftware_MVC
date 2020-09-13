using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity.System;

namespace DuyAmazone.Areas.Printify.Models
{
    public class AccountPrintifyModel
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

        public AccountPrintify ToObject()
        {
            return new AccountPrintify
            {
                Id = Id,
                UserName = UserName,
                Token = Token,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                UpdateDate = UpdateDate,
                UpdateBy = UpdateBy,
                IsActive = IsActive,
                Description = Description
            };
        }
    }
}