using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class TongKetSoQuyBll
    {
        private readonly TongKetSoQuyDal _tongKetSoQuyDal;

        public TongKetSoQuyBll()
        {
            _tongKetSoQuyDal = SingletonIpl.GetInstance<TongKetSoQuyDal>();
        }

        public List<DanhMucSoQuy> GetTongKetSoQuy(string tK, DateTime? fromDate, DateTime? toDate, string maDvcs, ref TongKetSoQuy tongKetSoQuy)
        {
            return _tongKetSoQuyDal.GetTongKetSoQuy(tK, fromDate, toDate, maDvcs,ref tongKetSoQuy);
        }
    }
}