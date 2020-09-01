using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class IncurredSalaryDal : BaseDal<ADOProvider>
    {
        public List<IncurredSalary> GetIncurredSalaries(long? employeeId, DateTime fromDate, DateTime toDate,
            bool calcSalary)
        {
            try
            {
                return
                    UnitOfWork.Procedure<IncurredSalary>("[hrm].[Get_IncurredSalaries]",
                        new
                        {
                            EmployeeId = employeeId,
                            FromDate = fromDate,
                            ToDate = toDate,
                            CalcSalary = calcSalary
                        }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<IncurredSalary>();
            }
        }

        public IncurredSalary GetIncurredSalary(string incurredSalaryId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<IncurredSalary>("[hrm].[Get_IncurredSalary]",
                        new {IncurredSalaryId = incurredSalaryId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(IncurredSalary incurredSalary)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_IncurredSalary]", new
                {
                    incurredSalary.Amount,
                    incurredSalary.CreateBy,
                    incurredSalary.CreateDate,
                    incurredSalary.Description,
                    incurredSalary.EmployeeId,
                    incurredSalary.IncurredSalaryId,
                    incurredSalary.SubmitDate,
                    incurredSalary.Title
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(IncurredSalary incurredSalary)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_IncurredSalary]", new
                {
                    incurredSalary.Amount,
                    incurredSalary.CreateBy,
                    incurredSalary.CreateDate,
                    incurredSalary.Description,
                    incurredSalary.EmployeeId,
                    incurredSalary.IncurredSalaryId,
                    incurredSalary.SubmitDate,
                    incurredSalary.Title
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string incurredSalaryId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_IncurredSalary]", new
                {
                    IncurredSalaryId = incurredSalaryId
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