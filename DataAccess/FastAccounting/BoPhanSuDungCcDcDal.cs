using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class BoPhanSuDungCcDcDal : BaseDal<FastADOProvider>
    {
        public List<BoPhanSuDungCcDc> GetBoPhan(string search)
        {
            try
            {
                return UnitOfWork.Procedure<BoPhanSuDungCcDc>("[dbo].[GetFrame]", new
                {
                    SqlTable = "v_dmbpcc",
                    StrFields = "[ma_bpcc],[ten_bpcc]",
                    OrderCol = "ma_bpcc         ",
                    Condition = " ((search like N'%" + search + "%')) ",
                    NBegin = 1,
                    NCount = short.MaxValue
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<BoPhanSuDungCcDc>();
            }
        }
    }
}