using System;

namespace Entity.Hrm
{
    public class Applicant
    {
        public string ApplicantId { get; set; }
        public string FullName { get; set; }
        public byte Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? CountryId { get; set; }
        public int? NationId { get; set; }
        public int? ReligionId { get; set; }
        public int CityBirthPlace { get; set; }
        public string PermanentAddress { get; set; }
        public string IdentityCardNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int ChanelId { get; set; }
        public int TrainingLevelId { get; set; }
        public long RecruitPlanId { get; set; }
        public DateTime CvDate { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateBy { get; set; }
        public string Description { get; set; }
    }
}