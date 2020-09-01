using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class TienGuiNganHangBll
    {
        private readonly TienGuiNganHangDal _tienGuiNganHangDal;

        public TienGuiNganHangBll()
        {
            _tienGuiNganHangDal = SingletonIpl.GetInstance<TienGuiNganHangDal>();
        }

        public List<TienGuiNganHang> TienGuiNganHangs(string tK, DateTime? fromDate, DateTime? toDate, string maDvcs
            ,int gopTk,ref TongKetTienGuiNh tongKetTienGuiNh)
        {
            return _tienGuiNganHangDal.TienGuiNganHangs(tK, fromDate, toDate, maDvcs, gopTk, ref tongKetTienGuiNh);
        }
    }
}