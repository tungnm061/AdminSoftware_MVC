using System.Collections.Generic;
using Core.Singleton;
using DataAccess.FastAccounting;
using Entity.FastAccounting;

namespace BusinessLogic.FastAccounting
{
    public class TonKhoBll
    {
        private readonly TonKhoDal _tonKhoDal;

        public TonKhoBll()
        {
            _tonKhoDal = SingletonIpl.GetInstance<TonKhoDal>();
        }

        public List<TonKho> TonKhos(string date, string filter, string condition)
        {
            return _tonKhoDal.TonKhos(date, filter, condition);
        }

        public List<TonKho> NhapXuatTons(string fromDate, string toDate, int tinhDc, string condition, string vtTonKho,
            string conditionSd)
        {
            return _tonKhoDal.NhapXuatTons(fromDate, toDate, tinhDc, condition, vtTonKho, conditionSd);
        }
    }
}