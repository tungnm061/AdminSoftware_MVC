using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class SalaryDal : BaseDal<ADOProvider>
    {
        public List<Salary> GetSalaries()
        {
            try
            {
                return UnitOfWork.Procedure<Salary>("[hrm].[Get_Salaries]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Salary>();
            }
        }

        public List<Salary> GetSalaries(long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Salary>("[hrm].[Get_Salaries_ByEmployeeId]", new {EmployeeId = employeeId})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Salary>();
            }
        }
        public Salary GetSalaryByEmployeeId(long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Salary>("[hrm].[Get_Salary_ByEmployeeId]", new { EmployeeId = employeeId })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new Salary();
            }
        }
        public Salary GetSalary(string salaryId)
        {
            try
            {
                return UnitOfWork.Procedure<Salary>("[hrm].[Get_Salary]", new {SalaryId = salaryId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(Salary salary)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_Salary]", new
                {
                    salary.ApplyDate,
                    salary.BasicCoefficient,
                    salary.BasicSalary,
                    salary.CreateBy,
                    salary.CreateDate,
                    salary.EmployeeId,
                    salary.PercentProfessional,
                    salary.ProfessionalCoefficient,
                    salary.ResponsibilityCoefficient,
                    salary.SalaryId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Salary salary)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Salary]", new
                {
                    salary.ApplyDate,
                    salary.BasicCoefficient,
                    salary.BasicSalary,
                    salary.CreateBy,
                    salary.CreateDate,
                    salary.EmployeeId,
                    salary.PercentProfessional,
                    salary.ProfessionalCoefficient,
                    salary.ResponsibilityCoefficient,
                    salary.SalaryId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string salaryId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Salary]", new
                {
                    SalaryId = salaryId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}