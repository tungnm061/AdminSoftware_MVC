using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class TimeSheetDal : BaseDal<ADOProvider>
    {
        public List<TimeSheet> GetTimeSheets(DateTime timeSheetDate)
        {
            try
            {
                return UnitOfWork.Procedure<TimeSheet>("[hrm].[Get_TimeSheets]", new
                {
                    TimeSheetDate = timeSheetDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TimeSheet>();
            }
        }
        public TimeSheet GetTimeSheetByEmployeeId(DateTime timeSheetDate,long employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<TimeSheet>("[hrm].[Get_TimeSheet_ByEmployeeId]", new
                {
                    TimeSheetDate = timeSheetDate,
                    EmployeeId    = employeeId
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new TimeSheet();
            }
        }
        public TimeSheet GetTimeSheetCheckDate(DateTime timeSheetDate)
        {
            try
            {
                return UnitOfWork.Procedure<TimeSheet>("[hrm].[Get_TimeSheet_CheckDate]", new
                {
                    TimeSheetDate = timeSheetDate
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new TimeSheet();
            }
        }

        public bool Update(TimeSheet timeSheet)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TimeSheetId", timeSheet.TimeSheetId);
                param.Add("@EmployeeId", timeSheet.EmployeeId);
                param.Add("@TimeSheetDate", timeSheet.TimeSheetDate);
                param.Add("@Checkin", timeSheet.Checkin);
                param.Add("@Checkout", timeSheet.Checkout);
                param.Add("@ShiftWorkId", timeSheet.ShiftWorkId);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_TimeSheet]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Inserts(DateTime timeSheetDate)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TimeSheetDate", timeSheetDate);
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_TimeSheets_ByDate]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}