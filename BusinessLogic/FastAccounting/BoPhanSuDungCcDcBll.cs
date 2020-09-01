using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class BoPhanSuDungCcDcBll
    {
        private readonly BoPhanSuDungCcDcDal _boPhanSuDungCcDcDal;

        public BoPhanSuDungCcDcBll()
        {
            _boPhanSuDungCcDcDal = SingletonIpl.GetInstance<BoPhanSuDungCcDcDal>();
        }

        public List<BoPhanSuDungCcDc> GetBoPhan(string search)
        {
            return _boPhanSuDungCcDcDal.GetBoPhan(search);
        }
    }
}