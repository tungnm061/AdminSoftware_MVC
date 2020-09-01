using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucDonViCoSoBll
    {
        private readonly DanhMucDonViCoSoDal _danhMucDonViCoSoDal;

        public DanhMucDonViCoSoBll()
        {
            _danhMucDonViCoSoDal = SingletonIpl.GetInstance<DanhMucDonViCoSoDal>();
        }

        public List<DanhMucDonViCoSo> GetDanhMucDonViCoSos()
        {
            return _danhMucDonViCoSoDal.GetDanhMucDonViCoSos();
        }
    }
}