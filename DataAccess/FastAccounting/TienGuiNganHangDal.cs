using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class TienGuiNganHangDal : BaseDal<FastADOProvider>
    {
        public List<TienGuiNganHang> TienGuiNganHangs(string tK, DateTime? fromDate, DateTime? toDate, string maDvcs,int gopTk,
            ref TongKetTienGuiNh tongKetTienGuiNh)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[dbo].[GLSO1D]", new
                {
                    tk = tK,
                    ngay_ct1 = fromDate,
                    ngay_ct2 = toDate,
                    gop_tk = gopTk,
                    ma_dvcs = maDvcs
                });
                var danhMucSoQuys = multi.Read<TienGuiNganHang>().ToList();
                if (danhMucSoQuys.Any())
                    tongKetTienGuiNh = multi.Read<TongKetTienGuiNh>().FirstOrDefault();
                return danhMucSoQuys;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TienGuiNganHang>();
            }
        }
    }
}