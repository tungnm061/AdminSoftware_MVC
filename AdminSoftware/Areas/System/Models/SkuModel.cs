using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entity.System;

namespace DuyAmazone.Areas.Printify.Models
{
    public class SkuModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime CreateDate { get; set; }

        public int CreateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? UpdateBy { get; set; }

        public string Description { get; set; }

        public int GmailId { get; set; }

        public Sku ToObject()
        {
            return new Sku
            {
                Id = Id,
                Code = Code,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                UpdateDate = UpdateDate,
                UpdateBy = UpdateBy,
                Description = Description,
                GmailId = GmailId
            };
        }
    }
}