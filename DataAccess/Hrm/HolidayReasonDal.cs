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
    public class HolidayReasonDal : BaseDal<ADOProvider>
    {
        public List<HolidayReason> GetHolidayReasons(bool? isActive)
        {
            try
            {
                return
                    UnitOfWork.Procedure<HolidayReason>("[hrm].[Get_HolidayReasons]", new {IsActive = isActive})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<HolidayReason>();
            }
        }

        public HolidayReason GetHolidayReason(int holidayReasonId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<HolidayReason>("[hrm].[Get_HolidayReason]",
                        new {HolidayReasonId = holidayReasonId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public HolidayReason GetHolidayReason(string reasonCode)
        {
            try
            {
                return
                    UnitOfWork.Procedure<HolidayReason>("[hrm].[Get_HolidayReason_ByCode]",
                        new {ReasonCode = reasonCode}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(HolidayReason holidayReason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", holidayReason.Description);
                param.Add("@HolidayReasonId", holidayReason.HolidayReasonId, DbType.Int32, ParameterDirection.Output);
                param.Add("@IsActive", holidayReason.IsActive);
                param.Add("@ReasonCode", holidayReason.ReasonCode);
                param.Add("@ReasonName", holidayReason.ReasonName);
                param.Add("@PercentSalary", holidayReason.PercentSalary);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_HolidayReason]", param))
                    return param.Get<int>("@HolidayReasonId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(HolidayReason holidayReason)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", holidayReason.Description);
                param.Add("@HolidayReasonId", holidayReason.HolidayReasonId);
                param.Add("@IsActive", holidayReason.IsActive);
                param.Add("@ReasonCode", holidayReason.ReasonCode);
                param.Add("@ReasonName", holidayReason.ReasonName);
                param.Add("@PercentSalary", holidayReason.PercentSalary);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_HolidayReason]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int holidayReasonId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_HolidayReason]",
                    new {HolidayReasonId = holidayReasonId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}