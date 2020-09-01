using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class HolidayDal : BaseDal<ADOProvider>
    {
        public List<Holiday> GetHolidays(int year, int month)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Holiday>("[hrm].[Get_Holiday_ByYearAndMonth]", new {Year = year, Month = month})
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Holiday>();
            }
        }

        public List<Holiday> GetHolidayByDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Holiday>("[hrm].[Get_Holiday_ByFromToDate]", new
                    {
                        FromDate = fromDate,
                        ToDate = toDate
                    })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Holiday>();
            }
        }
        public bool Delete(int year)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Holiday_ByYear]", new {Year = year});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Insert(List<Holiday> holidays)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_Holiday]",
                    new {XML = XmlHelper.SerializeXml<List<Holiday>>(holidays)});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}