using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class HolidayConfigBll
    {
        private readonly HolidayConfigDal _employeeHolidayDal;

        public HolidayConfigBll()
        {
            _employeeHolidayDal = SingletonIpl.GetInstance<HolidayConfigDal>();
        }

        public List<HolidayConfig> GetHolidayConfigs(int year,long employeeId)
        {
            return _employeeHolidayDal.GetHolidayConfigs(year, employeeId);
        }

        public HolidayConfig GetHolidayConfig(long employeeId, int year)
        {
            return _employeeHolidayDal.GetHolidayConfig(employeeId, year);
        }

        public bool Update(HolidayConfig employeeHoliday)
        {
            return _employeeHolidayDal.Update(employeeHoliday);
        }

        public bool Insert(HolidayConfig employeeHoliday)
        {
            return _employeeHolidayDal.Insert(employeeHoliday);
        }
    }
}