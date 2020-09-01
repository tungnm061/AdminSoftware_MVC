using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class TimeSheetOtDal : BaseDal<ADOProvider>
    {
        public List<TimeSheetOt> GetTimeSheetOts(DateTime? fromDate, DateTime? toDate, long? employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<TimeSheetOt>("[hrm].[Get_TimeSheetOts]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    EmployeeId = employeeId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TimeSheetOt>();
            }
        }

        public TimeSheetOt GetTimeSheetOt(string timeSheetOtId)
        {
            try
            {
                return UnitOfWork.Procedure<TimeSheetOt>("[hrm].[Get_TimeSheetOt]", new
                {
                    TimeSheetOtId = timeSheetOtId
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new TimeSheetOt();
            }
        }

        public bool Insert(TimeSheetOt model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TimeSheetOtId", model.TimeSheetOtId);
                param.Add("@EmployeeId", model.EmployeeId);
                param.Add("@DayDate", model.DayDate);
                param.Add("@Hours", model.Hours);
                param.Add("@CoefficientPoint", model.CoefficientPoint);
                param.Add("@DayPoints", model.DayPoints);
                param.Add("@CreateDate", model.CreateDate);
                param.Add("@Description", model.Description);
                return (UnitOfWork.ProcedureExecute("[hrm].[Insert_TimeSheetOt]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(TimeSheetOt model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TimeSheetOtId", model.TimeSheetOtId);
                param.Add("@EmployeeId", model.EmployeeId);
                param.Add("@DayDate", model.DayDate);
                param.Add("@Hours", model.Hours);
                param.Add("@CoefficientPoint", model.CoefficientPoint);
                param.Add("@DayPoints", model.DayPoints);
                param.Add("@CreateDate", model.CreateDate);
                param.Add("@Description", model.Description);
                return (UnitOfWork.ProcedureExecute("[hrm].[Update_TimeSheetOt]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string timeSheetOtId)
        {
            try
            {
                return (UnitOfWork.ProcedureExecute("[hrm].[Delete_TimeSheetOt]", new
                {
                    TimeSheetOtId = timeSheetOtId
                }));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}