using Core.Singleton;
using DataAccess.System;

namespace BusinessLogic.System
{
    public class AutoNumberBll
    {
        private readonly AutoNumberDal _autoNumberDal;

        public AutoNumberBll()
        {
            _autoNumberDal = SingletonIpl.GetInstance<AutoNumberDal>();
        }

        public string GetAutoNumber(string prefix)
        {
            return _autoNumberDal.GetAutoNumber(prefix);
        }
    }
}