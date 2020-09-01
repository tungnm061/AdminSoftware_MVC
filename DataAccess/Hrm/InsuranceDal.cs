using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class InsuranceDal : BaseDal<ADOProvider>
    {
        public List<Insurance> GetInsurances(bool? isActive)
        {
            try
            {
                return UnitOfWork.Procedure<Insurance>("[hrm].[Get_Insurances]", new {IsActive = isActive}).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Insurance>();
            }
        }

        public Insurance GetInsurance(long insuranceId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Insurance>("[hrm].[Get_Insurance]", new {InsuranceId = insuranceId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Insurance GetInsuranceByEmployeeId(long employeeId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Insurance>("[hrm].[Get_Insurance_ByEmployeeId]", new {EmployeeId = employeeId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Insurance insurance)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@InsuranceId", insurance.InsuranceId, DbType.Int64, ParameterDirection.Output);
                param.Add("@CityId", insurance.CityId);
                param.Add("@CreateBy", insurance.CreateBy);
                param.Add("@CreateDate", insurance.CreateDate);
                param.Add("@Description", insurance.Description);
                param.Add("@EmployeeId", insurance.EmployeeId);
                param.Add("@InsuranceNumber", insurance.InsuranceNumber);
                param.Add("@IsActive", insurance.IsActive);
                param.Add("@MonthBefore", insurance.MonthBefore);
                param.Add("@SubscriptionDate", insurance.SubscriptionDate);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_Insurance]", param))
                    return param.Get<long>("@InsuranceId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Insurance insurance)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Insurance]", new
                {
                    insurance.InsuranceId,
                    insurance.InsuranceNumber,
                    insurance.IsActive,
                    insurance.SubscriptionDate,
                    insurance.CityId,
                    insurance.Description,
                    insurance.EmployeeId,
                    insurance.CreateBy,
                    insurance.CreateDate,
                    insurance.MonthBefore
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long insuranceId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Insurance]", new
                {
                    InsuranceId = insuranceId
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