using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class ShiftWorkBll
    {
        private readonly ShiftWorkDal _shiftWorkDal;

        public ShiftWorkBll()
        {
            _shiftWorkDal = SingletonIpl.GetInstance<ShiftWorkDal>();
        }

        public List<ShiftWork> GetShiftWorks()
        {
            return _shiftWorkDal.GetShiftWorks();
        }

        public ShiftWork GetShiftWork(int shiftWorkId)
        {
            return _shiftWorkDal.GetShiftWork(shiftWorkId);
        }

        public ShiftWork GetShiftWork(string shiftWorkCode)
        {
            return _shiftWorkDal.GetShiftWork(shiftWorkCode);
        }

        public int Insert(ShiftWork shiftWork)
        {
            return _shiftWorkDal.Insert(shiftWork);
        }

        public bool Update(ShiftWork shiftWork)
        {
            return _shiftWorkDal.Update(shiftWork);
        }
    }
}