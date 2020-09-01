using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class InsuranceProcessDal : BaseDal<ADOProvider>
    {
        public List<InsuranceProcess> GetInsuranceProcesses(long employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<InsuranceProcess>("[hrm].[Get_InsuranceProcesses]",new
                {
                    EmployeeId = employeeId 
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<InsuranceProcess>();
            }
        }

        public InsuranceProcess GetInsuranceProcess(string insuranceProcessId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<InsuranceProcess>("[hrm].[Get_InsuranceProcess]",
                        new {InsuranceProcessId = insuranceProcessId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(InsuranceProcess insuranceProcess)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_InsuranceProcess]", new
                {
                    insuranceProcess.Amount,
                    insuranceProcess.CreateBy,
                    insuranceProcess.CreateDate,
                    insuranceProcess.Description,
                    insuranceProcess.FromDate,
                    insuranceProcess.InsuranceId,
                    insuranceProcess.InsuranceProcessId,
                    insuranceProcess.ToDate
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(InsuranceProcess insuranceProcess)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_InsuranceProcess]", new
                {
                    insuranceProcess.Amount,
                    insuranceProcess.CreateBy,
                    insuranceProcess.CreateDate,
                    insuranceProcess.Description,
                    insuranceProcess.FromDate,
                    insuranceProcess.InsuranceId,
                    insuranceProcess.InsuranceProcessId,
                    insuranceProcess.ToDate
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string insuranceProcessId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_InsuranceProcess]", new
                {
                    InsuranceProcessId = insuranceProcessId
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