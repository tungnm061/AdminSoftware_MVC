using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class DanhMucKhoBll
    {
        private readonly DanhMucKhoDal _danhMucKhoDal;

        public DanhMucKhoBll()
        {
            _danhMucKhoDal = SingletonIpl.GetInstance<DanhMucKhoDal>();
        }

        public List<DanhMucKho> GetDanhMucKhos(string maKho, string maDvcs)
        {
            return _danhMucKhoDal.GetDanhMucKhos(maKho, maDvcs);
        }
    }
}