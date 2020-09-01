using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucPhanLoaiTaiSanDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucPhanLoaiTaiSan> GetDanhMucBoPhanTaiSan(string search)
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucPhanLoaiTaiSan>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmplts",
                    StrFields = "[ma_loai],[ten_loai]",
                    OrderCol = "ma_loai         ",
                    Condition = " ((search like N'%" + search + "%')) ",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucPhanLoaiTaiSan>();
            }
        }
    }
}