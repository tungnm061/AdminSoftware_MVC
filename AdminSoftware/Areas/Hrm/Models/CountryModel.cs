using System.ComponentModel.DataAnnotations;
using Entity.System;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class CountryModel
    {
        [Required]
        public int CountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(255)]
        public string CountryName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Country ToObject()
        {
            return new Country
            {
                Description = Description,
                CountryCode = CountryCode,
                CountryId = CountryId,
                CountryName = CountryName
            };
        }
    }
}