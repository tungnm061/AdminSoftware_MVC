using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;
using Entity.Kpi;

namespace DataAccess.Hrm
{
    public class HolidayDetailDal : BaseDal<ADOProvider>
    {
        public List<HolidayDetail> GetHolidayDetails(DateTime fromDate,DateTime toDate,long employeeId)
        {
            try
            {
                return UnitOfWork.Procedure<HolidayDetail>("[hrm].[Get_HolidayDetails]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    EmployeeId = employeeId
                }).ToList();

            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<HolidayDetail>();
            }
        } 
        public bool Insert(string employeeHolidayId, List<HolidayDetail> holidayDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeHolidayId", employeeHolidayId);
                param.Add("@XML", XmlHelper.SerializeXml<List<HolidayDetail>>(holidayDetails));
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_HolidayDetails]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}