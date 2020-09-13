using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class OrderModel
    {
        public long OrderId { get; set; }

        public string OrderCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int? CountryId { get; set; }

        public int CityId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Region { get; set; }

        public string PostalZipCode { get; set; }

        public string Description { get; set; }

        public int GmailId { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public byte Status { get; set; }

        public bool IsActive { get; set; }
    }
}