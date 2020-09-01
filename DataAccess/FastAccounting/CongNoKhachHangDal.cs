using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class CongNoKhachHangDal : BaseDal<FastADOProvider>
    {
        public List<CongNoKhachHang> GetCongNoKhachHangs(string tK, DateTime? fromDate, DateTime? toDate, string advance)
        {
            try
            {
                return UnitOfWork.Procedure<CongNoKhachHang>("[dbo].[ARSO1T2]", new
                {
                    Tk = tK,
                    Ngay_ct1 = fromDate,
                    Ngay_ct2 = toDate,
                    Advance = advance
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<CongNoKhachHang>();
            }
        }
        public List<CongNoKhachHang> GetCongNoKhachHang(string maTk, string maKh, DateTime? fromDate, DateTime? toDate, string maDvcs,ref List<TongKetCongNo> tongKetCongNo)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[dbo].[ARSO1B]", new
                {
                    ma_tk = maTk,
                    ma_kh = maKh,
                    tu_ngay = fromDate,
                    den_ngay = toDate,
                    ma_dvcs = maDvcs,
                    chi_tiet= '0',
                    filter = "1=1"
                });
                tongKetCongNo = multi.Read<TongKetCongNo>().ToList();
                var congNoKhachHangs = new List<CongNoKhachHang>();
                if (tongKetCongNo.Any())
                     congNoKhachHangs = multi.Read<CongNoKhachHang>().ToList();
                return congNoKhachHangs;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<CongNoKhachHang>();
            }
        }
    }
}