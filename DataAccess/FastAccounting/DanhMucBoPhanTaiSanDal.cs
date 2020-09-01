using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucBoPhanTaiSanDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucBoPhanTaiSan> GetDanhMucBoPhanTaiSan(string search)
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucBoPhanTaiSan>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmbpts",
                    StrFields = "[ma_bpts],[ten_bpts]",
                    OrderCol = "ma_bpts         ",
                    Condition = " ((search like N'%" + search + "%')) ",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucBoPhanTaiSan>();
            }
        }
    }
}