using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucVatTuBll
    {
        private readonly DanhMucVatTuDal _danhMucVatTuDal;

        public DanhMucVatTuBll()
        {
            _danhMucVatTuDal = SingletonIpl.GetInstance<DanhMucVatTuDal>();
        }

        public List<DanhMucVatTu> GetDanhMucVatTus(string maVatTu)
        {
            return _danhMucVatTuDal.GetDanhMucVatTus(maVatTu);
        }
    }
}