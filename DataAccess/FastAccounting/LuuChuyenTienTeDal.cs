using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class LuuChuyenTienTeDal : BaseDal<FastADOProvider>
    {
        public List<LuuChuyenTienTe> GetLuuChuyenTienTes(DateTime? fromDate1, DateTime? toDate1, DateTime? fromDate2,
            DateTime? toDate2, int luyKe, string maDvcs)
        {
            try
            {
                return UnitOfWork.Procedure<LuuChuyenTienTe>("[dbo].[Gltcd]", new
                {
                    Ngay_ct1 = fromDate1,
                    Ngay_ct2 = toDate1,
                    Ngay_ct3 = fromDate2,
                    Ngay_ct4 = toDate2,
                    Luyke = luyKe,
                    Mau = "GLTCD7",
                    Ma_dvcs = maDvcs
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<LuuChuyenTienTe>();
            }
        }
    }
}