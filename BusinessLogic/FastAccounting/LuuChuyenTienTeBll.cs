using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class LuuChuyenTienTeBll
    {
        private readonly LuuChuyenTienTeDal _luuChuyenTienTeDal;

        public LuuChuyenTienTeBll()
        {
            _luuChuyenTienTeDal = SingletonIpl.GetInstance<LuuChuyenTienTeDal>();
        }

        public List<LuuChuyenTienTe> GetLuuChuyenTienTes(DateTime? fromDate1, DateTime? toDate1, DateTime? fromDate2,
            DateTime? toDate2, int luyKe, string maDvcs)
        {
            return _luuChuyenTienTeDal.GetLuuChuyenTienTes(fromDate1, toDate1, fromDate2, toDate2, luyKe, maDvcs);
        }
    }
}