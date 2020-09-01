using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class HolidayConfigDal : BaseDal<ADOProvider>
    {
        public List<HolidayConfig> GetHolidayConfigs(int year,long employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<HolidayConfig>("[hrm].[Get_HolidayConfigs]", new
                {
                    Year = year,
                    EmployeeId = employeeId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<HolidayConfig>();
            }
        }

        public HolidayConfig GetHolidayConfig(long employeeId, int year)
        {
            try
            {
                return
                    UnitOfWork.Procedure<HolidayConfig>("[hrm].[Get_HolidayConfig_ByEmployee]",
                        new {EmployeeId = employeeId, Year = year}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Update(HolidayConfig employeeHoliday)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Update_HolidayConfig]",
                    new
                    {
                        employeeHoliday.EmployeeId,
                        employeeHoliday.Year,
                        employeeHoliday.HolidayNumber
                    });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
        public bool Insert(HolidayConfig employeeHoliday)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_HolidayConfig]",
                    new
                    {
                        employeeHoliday.EmployeeId,
                        employeeHoliday.Year,
                        employeeHoliday.HolidayNumber
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