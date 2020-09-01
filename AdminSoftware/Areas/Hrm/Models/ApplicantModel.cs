using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class ApplicantModel
    {
        [Required]
        [StringLength(50)]
        public string ApplicantId { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        public byte Sex { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        public int? CountryId { get; set; }
        public int? NationId { get; set; }
        public int? ReligionId { get; set; }

        [Required]
        public int CityBirthPlace { get; set; }

        [Required]
        [StringLength(255)]
        public string PermanentAddress { get; set; }

        [StringLength(50)]
        public string IdentityCardNumber { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public int ChanelId { get; set; }

        [Required]
        public int TrainingLevelId { get; set; }

        [Required]
        public long RecruitPlanId { get; set; }

        [Required]
        public DateTime CvDate { get; set; }

        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public Applicant ToObject()
        {
            return new Applicant
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                FullName = FullName,
                CountryId = CountryId,
                PhoneNumber = PhoneNumber,
                RecruitPlanId = RecruitPlanId,
                CityBirthPlace = CityBirthPlace,
                DateOfBirth = DateOfBirth,
                Email = Email,
                IdentityCardNumber = IdentityCardNumber,
                NationId = NationId,
                PermanentAddress = PermanentAddress,
                ReligionId = ReligionId,
                TrainingLevelId = TrainingLevelId,
                Sex = Sex,
                ApplicantId = ApplicantId,
                ChanelId = ChanelId,
                CvDate = CvDate
            };
        }
    }
}