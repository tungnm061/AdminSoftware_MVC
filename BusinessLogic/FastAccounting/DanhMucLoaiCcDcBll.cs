using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucLoaiCcDcBll
    {
        private readonly DanhMucLoaiCcDcDal _danhMucLoaiCcDcDal;

        public DanhMucLoaiCcDcBll()
        {
            _danhMucLoaiCcDcDal = SingletonIpl.GetInstance<DanhMucLoaiCcDcDal>();
        }

        public List<DanhMucLoaiCcDc> GetLoaiCcDc(string search)
        {
            return _danhMucLoaiCcDcDal.GetLoaiCcDc(search);
        }
    }
}