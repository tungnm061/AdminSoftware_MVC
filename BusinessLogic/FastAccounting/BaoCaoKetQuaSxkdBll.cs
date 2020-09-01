using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class BaoCaoKetQuaSxkdBll
    {
        private readonly BaoCaoKetQuaSXKDDal _baoCaoKetQuaSxkdDal;

        public BaoCaoKetQuaSxkdBll()
        {
            _baoCaoKetQuaSxkdDal = SingletonIpl.GetInstance<BaoCaoKetQuaSXKDDal>();
        }

        public List<BaoCaoKetQuaSXKD> GetBaoCaoKetQuaSxkds(DateTime? fromDate1, DateTime? toDate1, DateTime? fromDate2,
            DateTime? toDate2, int luyKe, string maDvcs)
        {
            return _baoCaoKetQuaSxkdDal.GetBaoCaoKetQuaSxkds(fromDate1, toDate1, fromDate2, toDate2, luyKe, maDvcs);
        }
    }
}