using System;

namespace Entity.Hrm
{
    public class Employee
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }

        public string GenderString
        {
            get
            {
                if (Gender == 1)
                {
                    return "Nam";
                }
                if (Gender == 2)
                {
                    return "Nữ";
                }
                return "Khác";
            }
        }

        public string SpecialName { get; set; }
        public string Avatar { get; set; }
        public long DepartmentId { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int NationId { get; set; }
        public int ReligionId { get; set; }
        public byte? MaritalStatus { get; set; }
        public int? CityBirthPlace { get; set; }
        public int? CityNativeLand { get; set; }
        public string IdentityCardNumber { get; set; }
        public DateTime? IdentityCardDate { get; set; }
        public int? CityIdentityCard { get; set; }
        public string PermanentAddress { get; set; }
        public int? PermanentCity { get; set; }
        public int? PermanentDistrict { get; set; }
        public string TemperaryAddress { get; set; }
        public int? TemperaryCity { get; set; }
        public int? TemperaryDistrict { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public string PositionName { get; set; }
        public int? TrainingLevelId { get; set; }
        public string TrainingLevelName { get; set; }
        public string HealthStatus { get; set; }
        public DateTime? DateOfYouthUnionAdmission { get; set; }
        public string PlaceOfYouthUnionAdmission { get; set; }
        public DateTime? DateOfPartyAdmission { get; set; }
        public string PlaceOfPartyAdmission { get; set; }
        public string Skill { get; set; }
        public string Experience { get; set; }
        public string Description { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public byte Status { get; set; }
        public int? ShiftWorkId { get; set; }
        public DateTime? WorkedDate { get; set; }
        public int? EducationLevelId { get; set; }
        public string EducationLevelName { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int? CareerId { get; set; }
        public string CarrerName { get; set; }
        public string DepartmentName { get; set; }
        public string NationName { get; set; }
        public string ReligionName { get; set; }
        public string PermanentCityName { get; set; }
        public string PermanentDistrictName { get; set; }
        public string TimeSheetCode { get; set; }
        public int DepartmentCompany { get; set; }
        public int CategoryKpiId { get; set; }

        public string Address
        {
            get
            {
                if (PermanentAddress != null || PermanentDistrictName != null || PermanentCityName != null)
                {
                    return "" + PermanentAddress + " - " + PermanentDistrictName + " - " + PermanentCityName + "";
                }
                return "";
            }
        }

        public bool ViceDirectorManagement { get; set; }
    }
}