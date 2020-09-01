using System;
using System.ComponentModel.DataAnnotations;
using Entity.Hrm;

namespace AdminSoftware.Areas.Hrm.Models
{
    public class EmployeeModel
    {
        [Required]
        public long EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeCode { get; set; }

        [Required]
        [StringLength(255)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public byte Gender { get; set; }

        public string SpecialName { get; set; }
        public string Avatar { get; set; }

        [Required]
        public long DepartmentId { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int NationId { get; set; }

        [Required]
        public int ReligionId { get; set; }

        public byte? MaritalStatus { get; set; }
        public int? CityBirthPlace { get; set; }
        public int? CityNativeLand { get; set; }

        [StringLength(50)]
        public string IdentityCardNumber { get; set; }

        public DateTime? IdentityCardDate { get; set; }
        public int? CityIdentityCard { get; set; }

        [StringLength(500)]
        public string PermanentAddress { get; set; }

        public int? PermanentCity { get; set; }
        public int? PermanentDistrict { get; set; }

        [StringLength(500)]
        public string TemperaryAddress { get; set; }

        public int? TemperaryCity { get; set; }
        public int? TemperaryDistrict { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public int? TrainingLevelId { get; set; }

        [StringLength(500)]
        public string HealthStatus { get; set; }

        public DateTime? DateOfYouthUnionAdmission { get; set; }

        [StringLength(500)]
        public string PlaceOfYouthUnionAdmission { get; set; }

        public DateTime? DateOfPartyAdmission { get; set; }

        [StringLength(500)]
        public string PlaceOfPartyAdmission { get; set; }

        [StringLength(1000)]
        public string Skill { get; set; }

        [StringLength(1000)]
        public string Experience { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public int? ShiftWorkId { get; set; }
        public byte Status { get; set; }
        [Required]
        public byte EmployeeType { get; set; }
        public DateTime? WorkedDate { get; set; }
        [StringLength(50)]
        public string TimeSheetCode { get; set; }
        public int? EducationLevelId { get; set; }
        public int? SchoolId { get; set; }
        public int? CareerId { get; set; }
        public int DepartmentCompany { get; set; }
        public int CategoryKpiId { get; set; }
        public Employee ToObject()
        {
            return new Employee
            {
                Description = Description,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                IsActive = IsActive,
                FullName = FullName,
                CountryId = CountryId,
                DepartmentId = DepartmentId,
                Avatar = Avatar,
                PhoneNumber = PhoneNumber,
                EmployeeCode = EmployeeCode,
                CityBirthPlace = CityBirthPlace,
                CityIdentityCard = CityIdentityCard,
                CityNativeLand = CityNativeLand,
                DateOfBirth = DateOfBirth,
                DateOfPartyAdmission = DateOfPartyAdmission,
                DateOfYouthUnionAdmission = DateOfYouthUnionAdmission,
                Email = Email,
                EmployeeId = EmployeeId,
                Experience = Experience,
                Gender = Gender,
                HealthStatus = HealthStatus,
                IdentityCardDate = IdentityCardDate,
                IdentityCardNumber = IdentityCardNumber,
                MaritalStatus = MaritalStatus,
                NationId = NationId,
                PermanentAddress = PermanentAddress,
                PermanentCity = PermanentCity,
                PermanentDistrict = PermanentDistrict,
                PlaceOfPartyAdmission = PlaceOfPartyAdmission,
                PlaceOfYouthUnionAdmission = PlaceOfYouthUnionAdmission,
                PositionId = PositionId,
                ReligionId = ReligionId,
                Skill = Skill,
                SpecialName = SpecialName,
                TemperaryAddress = TemperaryAddress,
                TemperaryCity = TemperaryCity,
                TemperaryDistrict = TemperaryDistrict,
                TrainingLevelId = TrainingLevelId,
                ShiftWorkId = ShiftWorkId,
                Status = Status,
                WorkedDate = WorkedDate,
                CareerId = CareerId,
                EducationLevelId = EducationLevelId,
                SchoolId = SchoolId,
                TimeSheetCode = TimeSheetCode,
                DepartmentCompany = DepartmentCompany,
                CategoryKpiId = CategoryKpiId
            };
        }
    }
}