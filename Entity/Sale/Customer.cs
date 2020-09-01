using System;

namespace Entity.Sale
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string FullName { get; set; }
        public DateTime? IdentityCardDate { get; set; }
        public int? CityIdentityCard { get; set; }
        public string IdentityCard { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string BankAccountNumber { get; set; }
        public byte Status { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
    }
}