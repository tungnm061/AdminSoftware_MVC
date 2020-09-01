using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucKhoDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucKho> GetDanhMucKhos(string maKho, string maDvcs)
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucKho>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmkho",
                    StrFields = "[ma_kho],[ten_kho],[stt_ntxt],[tk_dl]",
                    OrderCol = "ma_kho           ",
                    Condition = "status = 1 and ma_kho like '%" + maKho + "%' and ma_dvcs like '%" + maDvcs + "%'",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucKho>();
            }
        }
    }
}