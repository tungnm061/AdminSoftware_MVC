using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class WorkDetailDal : BaseDal<ADOProvider>
    {
        public List<WorkDetail> GetWorkDetails(int? userId, int action, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<WorkDetail>("[kpi].[Get_WorkDetails]", new
                {
                    UserId = userId,
                    Action = action,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<WorkDetail>();
            }
        }
        public List<WorkDetail> GetWorkDetailsNextWeek(int? userId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<WorkDetail>("[kpi].[Get_WorkDetails_NextWeek]", new
                {
                    UserId = userId,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<WorkDetail>();
            }
        }
        public List<WorkDetail> GetWorkDetailsByPath(string path, int action, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<WorkDetail>("[kpi].[Get_WorkDetails_Path]", new
                {
                    Path = path,
                    Action = action,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<WorkDetail>();
            }
        }

        public List<StatisticalSugges> GetSuggesWorkKpis(DateTime? fromDate, DateTime? toDate, string path,int? userId)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalSugges>("[kpi].[Get_SuggesWorkKpis]", new
                {
                    Path = path,
                    FromDate = fromDate,
                    ToDate = toDate,
                    UserId = userId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<StatisticalSugges>();
            }
        }

        public List<StatisticalFinishWork> GetFinishWorkKpis(DateTime? fromDate, DateTime? toDate, string path)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalFinishWork>("[kpi].[Get_FinishWorkKpis]", new
                {
                    Path = path,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<StatisticalFinishWork>();
            }
        }
        public List<StatisticalFactorWork> GetFactorWorkKpisNew(DateTime? fromDate, DateTime? toDate,
            string path,int totalDay)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalFactorWork>("[kpi].[Get_FactorWorkKpisNew]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Path = path,
                    TotalDay = totalDay
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<StatisticalFactorWork>();
            }
        }
        public List<StatisticalFactorWork> GetFactorWorkKpis(DateTime? fromDate, DateTime? toDate, int totalDay,
            string path)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalFactorWork>("[kpi].[Get_FactorWorkKpis]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    TotalDay = totalDay,
                    Path = path
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<StatisticalFactorWork>();
            }
        }
        public StatisticalFactorWork GetFactorWorkKpi(DateTime? fromDate, DateTime? toDate, 
           long employeeId,int totalDay)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalFactorWork>("[kpi].[Get_FactorWorkKpiNew]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    EmployeeId = employeeId,
                    TotalDay = totalDay
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new StatisticalFactorWork
              ();
            }
        }
        public List<StatisticalComplain> GetComplainKpis(DateTime? fromDate, DateTime? toDate, string path)
        {
            try
            {
                return UnitOfWork.Procedure<StatisticalComplain>("[kpi].[Get_ComplainKpis]", new
                {
                    Path = path,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<StatisticalComplain>();
            }
        }

        public List<EmployeeUsefulHour> GetEmployeeUsefulHours(DateTime? fromDate, DateTime? toDate, long? departmentId)
        {
            try
            {
                return UnitOfWork.Procedure<EmployeeUsefulHour>("[kpi].[Get_EmployeeUsefulHours]", new
                {
                    DepartmentId = departmentId,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<EmployeeUsefulHour>();
            }
        }

        public WorkDetail GetWorkDetail(string workDetailId, int? workType)
        {
            try
            {
                return UnitOfWork.Procedure<WorkDetail>("[kpi].[Get_WorkDetail]", new
                {
                    WorkDetailId = workDetailId,
                    WorkType = workType
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new WorkDetail();
            }
        }
    }
}