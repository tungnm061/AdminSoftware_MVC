using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class HolidayBll
    {
        private readonly HolidayDal _holidayDal;

        public HolidayBll()
        {
            _holidayDal = SingletonIpl.GetInstance<HolidayDal>();
        }

        public List<Holiday> GetHolidayByDates(DateTime fromDate, DateTime toDate)
        {
            return _holidayDal.GetHolidayByDates(fromDate, toDate);
        }
        public List<Holiday> GetHolidays(int year, int month)
        {
            return _holidayDal.GetHolidays(year, month);
        }

        public bool Insert(List<Holiday> holidays, int year)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        if (!_holidayDal.Delete(year))
                        {
                            scope.Dispose();
                            return false;
                        }
                        if (!_holidayDal.Insert(holidays))
                        {
                            scope.Dispose();
                            return false;
                        }
                        scope.Complete();
                        return true;
                    }
                    catch (Exception)
                    {
                        scope.Dispose();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}