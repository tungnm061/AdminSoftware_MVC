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
    public class EmployeeHolidayDal : BaseDal<ADOProvider>
    {
        public List<EmployeeHoliday> GetEmployeeHolidays(DateTime? fromDate, DateTime? toDate,long? employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<EmployeeHoliday>("[hrm].[Get_EmployeeHolidays]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    EmployeeId = employeeId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<EmployeeHoliday>();
            }
        }

        public EmployeeHoliday GetEmployeeHoliday(string employeeHolidayId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeHolidayId", employeeHolidayId);
                var multi = UnitOfWork.ProcedureQueryMulti("[hrm].[Get_EmployeeHoliday]", param);
                var employeeHoliday = multi.Read<EmployeeHoliday>().FirstOrDefault();
                if (employeeHoliday != null)
                {
                    employeeHoliday.HolidayDetails = multi.Read<HolidayDetail>().ToList();
                }
                return employeeHoliday;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new EmployeeHoliday();
            }
        }

        public bool Insert(EmployeeHoliday model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeHolidayId", model.EmployeeHolidayId);
                param.Add("@HolidayReasonId", model.HolidayReasonId);
                param.Add("@FromDate", model.FromDate);
                param.Add("@ToDate", model.ToDate);
                param.Add("@Description", model.Description);
                param.Add("@EmployeeId", model.EmployeeId);
                param.Add("@CreateDate", model.CreateDate);
                return (UnitOfWork.ProcedureExecute("[hrm].[Insert_EmployeeHoliday]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(EmployeeHoliday model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeHolidayId", model.EmployeeHolidayId);
                param.Add("@HolidayReasonId", model.HolidayReasonId);
                param.Add("@FromDate", model.FromDate);
                param.Add("@ToDate", model.ToDate);
                param.Add("@Description", model.Description);
                param.Add("@EmployeeId", model.EmployeeId);
                param.Add("@CreateDate", model.CreateDate);
                return (UnitOfWork.ProcedureExecute("[hrm].[Update_EmployeeHoliday]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string employeeHolidayId)
        {
            try
            {
                return (UnitOfWork.ProcedureExecute("[hrm].[Delete_EmployeeHoliday]", new
                {
                    EmployeeHolidayId = employeeHolidayId
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