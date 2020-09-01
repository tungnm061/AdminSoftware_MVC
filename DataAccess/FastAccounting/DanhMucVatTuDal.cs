using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucVatTuDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucVatTu> GetDanhMucVatTus(string maVatTu)
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucVatTu>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmvt",
                    StrFields = "[ma_vt],[ten_vt],[tk_vt],[ma_tra_cuu]",
                    OrderCol = "ma_vt           ",
                    Condition = "status = 1 and ma_vt like '%" + maVatTu + "%'",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucVatTu>();
            }
        }
    }
}