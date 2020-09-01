using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class TaiSanCoDinhBll
    {
        private readonly TaiSanCoDinhDal _taiSanCoDinhDal;

        public TaiSanCoDinhBll()
        {
            _taiSanCoDinhDal = SingletonIpl.GetInstance<TaiSanCoDinhDal>();
        }

        public List<TaiSanCoDinh> GetTaiSanCoDinh(int year, int month, int firstOrLast, string maBpts,
            string loaiTs, string maDvcs)
        {
            return _taiSanCoDinhDal.GetTaiSanCoDinh(year, month, firstOrLast, maBpts, loaiTs, maDvcs);
        }
    }
}