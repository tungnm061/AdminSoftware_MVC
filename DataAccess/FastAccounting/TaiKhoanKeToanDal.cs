using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class TaiKhoanKeToanDal : BaseDal<FastADOProvider>
    {
        public List<TaiKhoanKeToan> GetTaiKhoanKeToans()
        {
            try
            {
                return UnitOfWork.Procedure<TaiKhoanKeToan>("[dbo].[GetFrame]", new
                {
                    SqlTable = "dmtk",
                    StrFields = "[tk],[ten_tk],[ma_nt],[loai_tk],[tk_me]",
                    OrderCol = "tk              ",
                    Condition = "1=1",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TaiKhoanKeToan>();
            }
        }
        public List<TaiKhoanKeToan> GetTaiKhoanCongNos()
        {
            try
            {
                return UnitOfWork.Procedure<TaiKhoanKeToan>("[dbo].[GetFrame]", new
                {
                    SqlTable = "dmtk",
                    StrFields = "[tk],[ten_tk],[ma_nt],[loai_tk],[tk_me]",
                    OrderCol = "tk              ",
                    Condition = "(tk_cn=1)",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TaiKhoanKeToan>();
            }
        }
    }
}