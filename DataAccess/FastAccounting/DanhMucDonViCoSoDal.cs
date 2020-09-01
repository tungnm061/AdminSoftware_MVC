using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class DanhMucDonViCoSoDal : BaseDal<FastADOProvider>
    {
        public List<DanhMucDonViCoSo> GetDanhMucDonViCoSos()
        {
            try
            {
                return UnitOfWork.Procedure<DanhMucDonViCoSo>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmdvcs",
                    StrFields = "[ma_dvcs],[ten_dvcs]",
                    OrderCol = "ma_dvcs           ",
                    Condition = "1=1",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<DanhMucDonViCoSo>();
            }
        }
    }
}