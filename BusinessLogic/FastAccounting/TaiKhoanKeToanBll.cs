using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class TaiKhoanKeToanBll
    {
        private readonly TaiKhoanKeToanDal _taiKhoanKeToanDal;

        public TaiKhoanKeToanBll()
        {
            _taiKhoanKeToanDal = SingletonIpl.GetInstance<TaiKhoanKeToanDal>();
        }

        public List<TaiKhoanKeToan> GetTaiKhoanKeToans()
        {
            return _taiKhoanKeToanDal.GetTaiKhoanKeToans();
        }

        public List<TaiKhoanKeToan> GetTaiKhoanCongNos()
        {
            return _taiKhoanKeToanDal.GetTaiKhoanCongNos();
        }
    }
}