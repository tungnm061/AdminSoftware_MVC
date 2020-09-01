using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucBoPhanTaiSanBll
    {
        private readonly DanhMucBoPhanTaiSanDal _danhMucBoPhanTaiSanDal;

        public DanhMucBoPhanTaiSanBll()
        {
            _danhMucBoPhanTaiSanDal = SingletonIpl.GetInstance<DanhMucBoPhanTaiSanDal>();
        }

        public List<DanhMucBoPhanTaiSan> GetDanhMucBoPhanTaiSan(string search)
        {
            return _danhMucBoPhanTaiSanDal.GetDanhMucBoPhanTaiSan(search);
        }
    }
}