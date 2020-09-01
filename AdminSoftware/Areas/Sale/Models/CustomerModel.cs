using System;
using System.ComponentModel.DataAnnotations;
using Entity.Sale;

namespace AdminSoftware.Areas.Sale.Models
{
    public class CustomerModel
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        public string CustomerCode { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        public DateTime? IdentityCardDate { get; set; }
        public int? CityIdentityCard { get; set; }
        public string IdentityCard { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string BankAccountNumber { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public int CreateBy { get; set; }

        public Customer ToObject()
        {
            return new Customer
            {
                CustomerId = CustomerId,
                CustomerCode = CustomerCode,
                FullName = FullName,
                IdentityCardDate = IdentityCardDate,
                CityIdentityCard = CityIdentityCard,
                IdentityCard = IdentityCard,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Description = Description,
                Address = Address,
                CityId = CityId,
                DistrictId = DistrictId,
                TaxCode = TaxCode,
                CompanyName = CompanyName,
                BankAccountNumber = BankAccountNumber,
                Status = Status,
                CreateDate = CreateDate,
                CreateBy = CreateBy
            };
        }
    }
}