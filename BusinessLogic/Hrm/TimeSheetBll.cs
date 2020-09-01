using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class TimeSheetBll
    {
        private readonly TimeSheetDal _timeSheetDal;

        public TimeSheetBll()
        {
            _timeSheetDal = SingletonIpl.GetInstance<TimeSheetDal>();
        }

        public TimeSheet GetTimeSheetByEmployeeId(DateTime timeSheetDate, long employeeId)
        {
            return _timeSheetDal.GetTimeSheetByEmployeeId(timeSheetDate, employeeId);
        }

        public List<TimeSheet> GetTimeSheets(DateTime timeSheetDate)
        {
            return _timeSheetDal.GetTimeSheets(timeSheetDate);
        }

        public TimeSheet GetTimeSheetCheckDate(DateTime timeSheetDate)
        {
            return _timeSheetDal.GetTimeSheetCheckDate(timeSheetDate);
        }

        public bool Update(TimeSheet timeSheet)
        {
            return _timeSheetDal.Update(timeSheet);
        }

        public bool UpdateList(List<TimeSheet> timeSheets)
        {
            if (timeSheets == null || !timeSheets.Any())
                return false;
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var timeSheet in timeSheets)
                    {
                        if (!_timeSheetDal.Update(timeSheet))
                        {
                            scope.Dispose();
                            return false;
                        }
                    }
                    scope.Complete();
                    return true;
                }
                catch (Exception ex)
                {
                    Logging.PutError(ex.Message, ex);
                    scope.Dispose();
                    return false;
                }
            }
        }

        public bool Inserts(DateTime timeSheetDate)
        {
            return _timeSheetDal.Inserts(timeSheetDate);
        }
    }
}