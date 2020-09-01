using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucPhanLoaiTaiSanBll
    {
        private readonly DanhMucPhanLoaiTaiSanDal _danhMucPhanLoaiTaiSanDal;

        public DanhMucPhanLoaiTaiSanBll()
        {
            _danhMucPhanLoaiTaiSanDal = SingletonIpl.GetInstance<DanhMucPhanLoaiTaiSanDal>();
        }

        public List<DanhMucPhanLoaiTaiSan> GetDanhMucBoPhanTaiSan(string search)
        {
            return _danhMucPhanLoaiTaiSanDal.GetDanhMucBoPhanTaiSan(search);
        }
    }
}