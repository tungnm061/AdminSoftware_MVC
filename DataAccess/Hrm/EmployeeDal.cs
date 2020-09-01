using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;
using Entity.System;

namespace DataAccess.Hrm
{
    public class EmployeeDal : BaseDal<ADOProvider>
    {
        public List<Employee> GetEmployees(bool? isActive)
        {
            try
            {
                return UnitOfWork.Procedure<Employee>("[hrm].[Get_Employees]", new {IsActive = isActive}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }
        public List<Employee> GetEmployeesByPath(string path)
        {
            try
            {
                return UnitOfWork.Procedure<Employee>("[hrm].[Get_Employee_ByPath]", new { Path = path }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }
        
        public List<Employee> GetEmployeesTimeSheet(bool? isActive)
        {
            try
            {
                return UnitOfWork.Procedure<Employee>("[hrm].[Get_Employees_TimeSheet]", new { IsActive = isActive }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }
        public List<Employee> GetEmployeesSalary(bool? isActive)
        {
            try
            {
                return UnitOfWork.Procedure<Employee>("[hrm].[Get_Employees_Salary]", new { IsActive = isActive }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }
        public List<Employee> GetEmployeesByKeyword(string keyword)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Employee>("[hrm].[Get_Employees_ByKeyword]", new {Keyword = keyword}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }

        public List<Employee> GetEmployeesForInsurance(bool? join)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Employee>("[hrm].[Get_Employees_ForInsurance]", new {Join = join}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Employee>();
            }
        }
        public List<User> GetEmployeesByDepartmentAndPosition(byte? position,long? departmentId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<User>("[hrm].[Get_Employee_ByDepartmentIdAndPosition]", new
                    {
                        DepartmentId = departmentId,
                        Position = position
                    }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<User>();
            }
        }

        public Employee GetEmployee(long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Employee>("[hrm].[Get_Employee]", new {EmployeeId = employeeId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }
        public Employee GetEmployeeByTimeSheetCode(string timesheetCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Employee>("[hrm].[Get_Employee_ByTimeSheetCode]", new { TimesheetCode = timesheetCode })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Employee GetEmployee(string employeeCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Employee>("[hrm].[Get_Employee_ByEmployeeCode]",
                        new {EmployeeCode = employeeCode}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Employee employee, ref string employeeCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employee.EmployeeId, DbType.Int64, ParameterDirection.Output);
                param.Add("@EmployeeCode", employee.EmployeeCode, DbType.String, ParameterDirection.InputOutput, 50);
                param.Add("@FullName", employee.FullName);
                param.Add("@DateOfBirth", employee.DateOfBirth);
                param.Add("@Gender", employee.Gender);
                param.Add("@SpecialName", employee.SpecialName);
                param.Add("@Avatar", employee.Avatar);
                param.Add("@DepartmentId", employee.DepartmentId);
                param.Add("@CountryId", employee.CountryId);
                param.Add("@NationId", employee.NationId);
                param.Add("@ReligionId", employee.ReligionId);
                param.Add("@MaritalStatus", employee.MaritalStatus);
                param.Add("@CityBirthPlace", employee.CityBirthPlace);
                param.Add("@CityNativeLand", employee.CityNativeLand);
                param.Add("@IdentityCardNumber", employee.IdentityCardNumber);
                param.Add("@IdentityCardDate", employee.IdentityCardDate);
                param.Add("@CityIdentityCard", employee.CityIdentityCard);
                param.Add("@PermanentAddress", employee.PermanentAddress);
                param.Add("@PermanentCity", employee.PermanentCity);
                param.Add("@PermanentDistrict", employee.PermanentDistrict);
                param.Add("@TemperaryAddress", employee.TemperaryAddress);
                param.Add("@TemperaryCity", employee.TemperaryCity);
                param.Add("@TemperaryDistrict", employee.TemperaryDistrict);
                param.Add("@Email", employee.Email);
                param.Add("@PhoneNumber", employee.PhoneNumber);
                param.Add("@PositionId", employee.PositionId);
                param.Add("@TrainingLevelId", employee.TrainingLevelId);
                param.Add("@HealthStatus", employee.HealthStatus);
                param.Add("@DateOfYouthUnionAdmission", employee.DateOfYouthUnionAdmission);
                param.Add("@PlaceOfYouthUnionAdmission", employee.PlaceOfYouthUnionAdmission);
                param.Add("@DateOfPartyAdmission", employee.DateOfPartyAdmission);
                param.Add("@PlaceOfPartyAdmission", employee.PlaceOfPartyAdmission);
                param.Add("@Skill", employee.Skill);
                param.Add("@Experience", employee.Experience);
                param.Add("@Description", employee.Description);
                param.Add("@CreateBy", employee.CreateBy);
                param.Add("@CreateDate", employee.CreateDate);
                param.Add("@IsActive", employee.IsActive);
                param.Add("@Status", employee.Status);
                param.Add("@ShiftWorkId", employee.ShiftWorkId);
                param.Add("@WorkedDate", employee.WorkedDate);
                param.Add("@EducationLevelId", employee.EducationLevelId);
                param.Add("@CareerId", employee.CareerId);
                param.Add("@SchoolId", employee.SchoolId);
                param.Add("@TimeSheetCode", employee.TimeSheetCode);
                param.Add("@DepartmentCompany", employee.DepartmentCompany);
                param.Add("@CategoryKpiId", employee.CategoryKpiId);
                //param.Add("@ViceDirectorManagement", employee.ViceDirectorManagement);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Employee]", param))
                {
                    employeeCode = param.Get<string>("@EmployeeCode");
                    return param.Get<long>("@EmployeeId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Employee employee)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employee.EmployeeId);
                param.Add("@FullName", employee.FullName);
                param.Add("@DateOfBirth", employee.DateOfBirth);
                param.Add("@Gender", employee.Gender);
                param.Add("@SpecialName", employee.SpecialName);
                param.Add("@Avatar", employee.Avatar);
                param.Add("@DepartmentId", employee.DepartmentId);
                param.Add("@CountryId", employee.CountryId);
                param.Add("@NationId", employee.NationId);
                param.Add("@ReligionId", employee.ReligionId);
                param.Add("@MaritalStatus", employee.MaritalStatus);
                param.Add("@CityBirthPlace", employee.CityBirthPlace);
                param.Add("@CityNativeLand", employee.CityNativeLand);
                param.Add("@IdentityCardNumber", employee.IdentityCardNumber);
                param.Add("@IdentityCardDate", employee.IdentityCardDate);
                param.Add("@CityIdentityCard", employee.CityIdentityCard);
                param.Add("@PermanentAddress", employee.PermanentAddress);
                param.Add("@PermanentCity", employee.PermanentCity);
                param.Add("@PermanentDistrict", employee.PermanentDistrict);
                param.Add("@TemperaryAddress", employee.TemperaryAddress);
                param.Add("@TemperaryCity", employee.TemperaryCity);
                param.Add("@TemperaryDistrict", employee.TemperaryDistrict);
                param.Add("@Email", employee.Email);
                param.Add("@PhoneNumber", employee.PhoneNumber);
                param.Add("@PositionId", employee.PositionId);
                param.Add("@TrainingLevelId", employee.TrainingLevelId);
                param.Add("@HealthStatus", employee.HealthStatus);
                param.Add("@DateOfYouthUnionAdmission", employee.DateOfYouthUnionAdmission);
                param.Add("@PlaceOfYouthUnionAdmission", employee.PlaceOfYouthUnionAdmission);
                param.Add("@DateOfPartyAdmission", employee.DateOfPartyAdmission);
                param.Add("@PlaceOfPartyAdmission", employee.PlaceOfPartyAdmission);
                param.Add("@Skill", employee.Skill);
                param.Add("@Experience", employee.Experience);
                param.Add("@Description", employee.Description);
                param.Add("@IsActive", employee.IsActive);
                param.Add("@Status", employee.Status);
                param.Add("@ShiftWorkId", employee.ShiftWorkId);
                param.Add("@WorkedDate", employee.WorkedDate);
                param.Add("@EducationLevelId", employee.EducationLevelId);
                param.Add("@CareerId", employee.CareerId);
                param.Add("@SchoolId", employee.SchoolId);
                param.Add("@TimeSheetCode", employee.TimeSheetCode);
                param.Add("@DepartmentCompany", employee.DepartmentCompany);
                param.Add("@CategoryKpiId", employee.CategoryKpiId);
                //param.Add("@ViceDirectorManagement", employee.ViceDirectorManagement);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Employee]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long employeeId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Employee]", new {EmployeeId = employeeId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}