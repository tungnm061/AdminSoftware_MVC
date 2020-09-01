using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class KhachHangDal : BaseDal<FastADOProvider>
    {
        public List<KhachHang> GetKhachHangs()
        {
            try
            {
                return UnitOfWork.Procedure<KhachHang>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmkh",
                    StrFields = "[ma_kh],[ten_kh]",
                    OrderCol = "ma_kh",
                    Condition = "status = 1",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<KhachHang>();
            }
        }
    }
}
