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
    public class EmployeeHolidayBll
    {
        private readonly EmployeeHolidayDal _employeeHolidayDal;
        private readonly HolidayDetailDal _holidayDetailDal;
        public EmployeeHolidayBll()
        {
            _employeeHolidayDal = SingletonIpl.GetInstance<EmployeeHolidayDal>();
            _holidayDetailDal = SingletonIpl.GetInstance<HolidayDetailDal>();
        }

        public List<EmployeeHoliday> GetEmployeeHolidays(DateTime? fromDate, DateTime? toDate,long? employeeId)
        {
            return _employeeHolidayDal.GetEmployeeHolidays(fromDate, toDate, employeeId);
        }

        public EmployeeHoliday GetEmployeeHoliday(string employeeHolidayId)
        {
            return _employeeHolidayDal.GetEmployeeHoliday(employeeHolidayId);
        }

        public bool Insert(EmployeeHoliday model)
        {
            try
            {
                if (model == null || !model.HolidayDetails.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_employeeHolidayDal.Insert(model))
                    {
                        if (_holidayDetailDal.Insert(model.EmployeeHolidayId, model.HolidayDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
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
                if (model == null || !model.HolidayDetails.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_employeeHolidayDal.Update(model))
                    {
                        if (_holidayDetailDal.Insert(model.EmployeeHolidayId, model.HolidayDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string employeeHolidayId)
        {
            return _employeeHolidayDal.Delete(employeeHolidayId);
        }

        public List<HolidayDetail> GetHolidayDetails(DateTime fromDate, DateTime toDate, long employeeId)
        {
            return _holidayDetailDal.GetHolidayDetails(fromDate, toDate, employeeId);
        }
    }
}