using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class TongKetSoQuyDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucSoQuy> GetTongKetSoQuy(string tK, DateTime? fromDate, DateTime? toDate, string maDvcs,ref TongKetSoQuy tongKetSoQuy)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[dbo].[Caso1]", new
                {
                    tk = tK,
                    ngay_ct1 = fromDate,
                    ngay_ct2 = toDate,
                    ma_dvcs = maDvcs
                });
                var danhMucSoQuys = multi.Read<DanhMucSoQuy>().ToList();
                if (danhMucSoQuys.Any())
                    tongKetSoQuy = multi.Read<TongKetSoQuy>().FirstOrDefault();
                return danhMucSoQuys;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucSoQuy>();
            }
        }
    }
}