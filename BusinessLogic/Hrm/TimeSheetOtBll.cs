using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class TimeSheetOtBll
    {
        private readonly TimeSheetOtDal _timeSheetOtDal;

        public TimeSheetOtBll()
        {
            _timeSheetOtDal = SingletonIpl.GetInstance<TimeSheetOtDal>();
        }

        public List<TimeSheetOt> GetTimeSheetOts(DateTime? fromDate, DateTime? toDate, long? employeeId)
        {
            return _timeSheetOtDal.GetTimeSheetOts(fromDate, toDate, employeeId);
        }

        public TimeSheetOt GetTimeSheetOt(string timeSheetOtId)
        {
            return _timeSheetOtDal.GetTimeSheetOt(timeSheetOtId);
        }

        public bool Insert(TimeSheetOt model)
        {
            return _timeSheetOtDal.Insert(model);
        }

        public bool Update(TimeSheetOt model)
        {
            return _timeSheetOtDal.Update(model);
        }

        public bool Delete(string timeSheetOtId)
        {
            return _timeSheetOtDal.Delete(timeSheetOtId);
        }
    }
}