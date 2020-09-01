using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class AssignWorkBll
    {
        private readonly AssignWorkDal _assignWorkDal;

        public AssignWorkBll()
        {
            _assignWorkDal = SingletonIpl.GetInstance<AssignWorkDal>();
        }

        public List<AssignWork> GetAssignWorkByFollows(int employeeId)
        {
            return _assignWorkDal.GetAssignWorkByFollows(employeeId);
        }
        public List<AssignWork> GetAssignWorkByUserIds(long? createBy, long? assignBy)
        {
            return _assignWorkDal.GetAssignWorkByUserIds(createBy, assignBy);
        }
        public List<AssignWork> GetAssignWorks(int? status, long? createBy, long? assignBy, DateTime? fromDate,
            DateTime? toDate)
        {
            return _assignWorkDal.GetAssignWorks(status, createBy, assignBy, fromDate, toDate);
        }

        public AssignWork GetAssignWork(string assignWorkId)
        {
            return _assignWorkDal.GetAssignWork(assignWorkId);
        }

        public bool Insert(AssignWork assignWork)
        {
            return _assignWorkDal.Insert(assignWork);
        }

        public bool Update(AssignWork assignWork)
        {
            return _assignWorkDal.Update(assignWork);
        }

        public bool Delete(string assignWorkId)
        {
            return _assignWorkDal.Delete(assignWorkId);
        }
    }
}