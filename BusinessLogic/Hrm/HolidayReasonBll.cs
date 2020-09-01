using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class HolidayReasonBll
    {
        private readonly HolidayReasonDal _holidayReasonDal;

        public HolidayReasonBll()
        {
            _holidayReasonDal = SingletonIpl.GetInstance<HolidayReasonDal>();
        }

        public List<HolidayReason> GetHolidayReasons(bool? isActive)
        {
            return _holidayReasonDal.GetHolidayReasons(isActive);
        }

        public HolidayReason GetHolidayReason(int holidayReasonId)
        {
            return _holidayReasonDal.GetHolidayReason(holidayReasonId);
        }

        public HolidayReason GetHolidayReason(string reasonCode)
        {
            return _holidayReasonDal.GetHolidayReason(reasonCode);
        }

        public int Insert(HolidayReason holidayReason)
        {
            return _holidayReasonDal.Insert(holidayReason);
        }

        public bool Update(HolidayReason holidayReason)
        {
            return _holidayReasonDal.Update(holidayReason);
        }

        public bool Delete(int holidayReasonId)
        {
            return _holidayReasonDal.Delete(holidayReasonId);
        }
    }
}