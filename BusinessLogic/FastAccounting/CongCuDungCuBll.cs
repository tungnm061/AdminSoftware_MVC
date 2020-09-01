using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class CongCuDungCuBll
    {
        private readonly CongCuDungCuDal _congCuDungCuDal;

        public CongCuDungCuBll()
        {
            _congCuDungCuDal = SingletonIpl.GetInstance<CongCuDungCuDal>();
        }

        public List<CongCuDungCu> GetCongCuDungCu(int year, int month, int firstOrLast, string maBpCc,
            string loaiCc, string maDvcs)
        {
            return _congCuDungCuDal.GetCongCuDungCu(year, month, firstOrLast, maBpCc, loaiCc, maDvcs);
        }
    }
}