using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Helper.ExtendedAttributes;

namespace Entity.Sale
{
    public class Order
    {
        [LocalizeRequired]
        public long OrderId { get; set; }

        [LocalizeRequired]
        [DisplayName("Mã đơn hàng")]
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

        [LocalizeRequired]
        [DisplayName("Tài khoản gmail")]
        public int GmailId { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [LocalizeRequired]
        [DisplayName("Trạng thái")]
        public byte Status { get; set; }

        public bool IsActive { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
        public DateTime? FinishDate { get; set; }
        [LocalizeRequired]
        [DisplayName("Nhà sản xuất")]
        public int? ProducerId { get; set; }
        public byte TypeMoney { get; set; }
        [LocalizeRequired]
        [DisplayName("Tiền ship")]
        public decimal? ShipMoney { get; set; }
        public decimal TotalPrince { get; set; }
        public decimal? RateMoney { get; set; }
        public string TrackingCode { get; set; }
        [LocalizeRequired]
        [DisplayName("Ngày đặt đơn")]
        public DateTime StartDate { get; set; }

    }
}
