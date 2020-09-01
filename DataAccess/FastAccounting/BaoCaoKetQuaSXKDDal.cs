using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

// ReSharper disable  InconsistentNaming

namespace DataAccess.FastAccounting
{
    public class BaoCaoKetQuaSXKDDal : BaseDal<FastADOProvider>
    {
        public List<BaoCaoKetQuaSXKD> GetBaoCaoKetQuaSxkds(DateTime? fromDate1, DateTime? toDate1, DateTime? fromDate2,
            DateTime? toDate2, int luyKe, string maDvcs)
        {
            try
            {
                return UnitOfWork.Procedure<BaoCaoKetQuaSXKD>("[dbo].[Gltcc]", new
                {
                    Ngay_ct1 = fromDate1,
                    Ngay_ct2 = toDate1,
                    Ngay_ct3 = fromDate2,
                    Ngay_ct4 = toDate2,
                    Luyke = luyKe,
                    Mau = "GLTCC48",
                    Ma_dvcs = maDvcs
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<BaoCaoKetQuaSXKD>();
            }
        }
    }
}