using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Sale
{
    public class Order
    {
        public long OrderId { get; set; }

        public string OrderCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int? CountryId { get; set; }

        public string City { get; set; }

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

        public List<OrderDetail> OrderDetails { get; set; }
        public DateTime? FinishDate { get; set; }
        public int ProducerId { get; set; }
        public byte TypeMoney { get; set; }
        public decimal ShipMoney { get; set; }
        public decimal TotalPrince { get; set; }
        public decimal? RateMoney { get; set; }
        public string TrackingCode { get; set; }
        public DateTime StartDate { get; set; }

    }
}
