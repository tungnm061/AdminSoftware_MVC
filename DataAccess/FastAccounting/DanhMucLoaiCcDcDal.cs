using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucLoaiCcDcDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucLoaiCcDc> GetLoaiCcDc(string search)
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucLoaiCcDc>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmplcc",
                    StrFields = "[loai_cc],[ten_loai]",
                    OrderCol = "loai_cc         ",
                    Condition = " ((search like N'%" + search + "%')) ",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucLoaiCcDc>();
            }
        }
    }
}