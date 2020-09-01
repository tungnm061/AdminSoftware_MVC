using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class CongNoKhachHangBll
    {
        private readonly CongNoKhachHangDal _congNoKhachHangDal;

        public CongNoKhachHangBll()
        {
            _congNoKhachHangDal = SingletonIpl.GetInstance<CongNoKhachHangDal>();
        }

        public List<CongNoKhachHang> GetCongNoKhachHang(string maTk, string maKh, DateTime? fromDate, DateTime? toDate,
            string maDvcs, ref List<TongKetCongNo> tongKetCongNo)
        {
            return _congNoKhachHangDal.GetCongNoKhachHang(maTk, maKh, fromDate, toDate, maDvcs, ref tongKetCongNo);
        }
        public List<CongNoKhachHang> GetCongNoKhachHangs(string tK, DateTime? fromDate, DateTime? toDate, string advance)
        {
            return _congNoKhachHangDal.GetCongNoKhachHangs(tK, fromDate, toDate, advance);
        }
    }
}