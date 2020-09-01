using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class WorkDetailBll
    {
        private readonly WorkDetailDal _workDetailDal;

        public WorkDetailBll()
        {
            _workDetailDal = SingletonIpl.GetInstance<WorkDetailDal>();
        }

        public List<StatisticalSugges> GetSuggesWorkKpis(DateTime? fromDate, DateTime? toDate,string path, int? userId)
        {
            return _workDetailDal.GetSuggesWorkKpis(fromDate, toDate, path, userId);
        }

        public List<StatisticalFinishWork> GetFinishWorkKpis(DateTime? fromDate, DateTime? toDate, string path)
        {
            return _workDetailDal.GetFinishWorkKpis(fromDate, toDate, path);
        }

        public List<StatisticalFactorWork> GetFactorWorkKpisNew(DateTime? fromDate, DateTime? toDate,
            string path,int totalDay)
        {
            return _workDetailDal.GetFactorWorkKpisNew(fromDate, toDate, path, totalDay);
        }
        public List<StatisticalFactorWork> GetFactorWorkKpis(DateTime? fromDate, DateTime? toDate, int totalDay,
           string path)
        {
            return _workDetailDal.GetFactorWorkKpis(fromDate, toDate, totalDay, path);
        }

        public StatisticalFactorWork GetFactorWorkKpi(DateTime? fromDate, DateTime? toDate, 
           long employeeId, int totalDay)
        {
            return _workDetailDal.GetFactorWorkKpi(fromDate, toDate, employeeId, totalDay);
        }
        public List<StatisticalComplain> GetComplainKpis(DateTime? fromDate, DateTime? toDate, string path)
        {
            return _workDetailDal.GetComplainKpis(fromDate, toDate, path);
        }

        public List<WorkDetail> GetWorkDetails(int? userId, int action, DateTime? fromDate, DateTime? toDate)
        {
            return _workDetailDal.GetWorkDetails(userId, action, fromDate, toDate);
        }

        public List<WorkDetail> GetWorkDetailsNextWeek(int? userId, DateTime? fromDate, DateTime? toDate)
        {
            return _workDetailDal.GetWorkDetailsNextWeek(userId, fromDate, toDate);
        }
        public List<WorkDetail> GetWorkDetailsByPath(string path, int action, DateTime? fromDate, DateTime? toDate)
        {
            return _workDetailDal.GetWorkDetailsByPath(path, action, fromDate, toDate);
        }
        public WorkDetail GetWorkDetail(string workDetailId, int? workType)
        {
            return _workDetailDal.GetWorkDetail(workDetailId, workType);
        }

        public List<EmployeeUsefulHour> GetEmployeeUsefulHours(DateTime? fromDate, DateTime? toDate, long? departmentId)
        {
            return _workDetailDal.GetEmployeeUsefulHours(fromDate, toDate, departmentId);
        }
    }
}