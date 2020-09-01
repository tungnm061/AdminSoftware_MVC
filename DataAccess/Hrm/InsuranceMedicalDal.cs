using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class InsuranceMedicalDal : BaseDal<ADOProvider>
    {
        public List<InsuranceMedical> GetInsuranceMedicals(int year,long? employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<InsuranceMedical>("[hrm].[Get_InsuranceMedicals]", new
                    {
                        Year = year,
                        EmployeeId = employeeId

                    }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<InsuranceMedical>();
            }
        }

        public InsuranceMedical GetInsuranceMedical(string insuranceMedicalId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<InsuranceMedical>("[hrm].[Get_InsuranceMedical]",
                        new {InsuranceMedicalId = insuranceMedicalId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(InsuranceMedical insuranceMedical)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_InsuranceMedical]", new
                {
                    insuranceMedical.Amount,
                    insuranceMedical.CityId,
                    insuranceMedical.CreateBy,
                    insuranceMedical.CreateDate,
                    insuranceMedical.Description,
                    insuranceMedical.EmployeeId,
                    insuranceMedical.ExpiredDate,
                    insuranceMedical.InsuranceMedicalId,
                    insuranceMedical.InsuranceMedicalNumber,
                    insuranceMedical.MedicalId,
                    insuranceMedical.StartDate
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(InsuranceMedical insuranceMedical)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_InsuranceMedical]", new
                {
                    insuranceMedical.Amount,
                    insuranceMedical.CityId,
                    insuranceMedical.CreateBy,
                    insuranceMedical.CreateDate,
                    insuranceMedical.Description,
                    insuranceMedical.EmployeeId,
                    insuranceMedical.ExpiredDate,
                    insuranceMedical.InsuranceMedicalId,
                    insuranceMedical.InsuranceMedicalNumber,
                    insuranceMedical.MedicalId,
                    insuranceMedical.StartDate
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string insuranceMedicalId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_InsuranceMedical]", new
                {
                    InsuranceMedicalId = insuranceMedicalId
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