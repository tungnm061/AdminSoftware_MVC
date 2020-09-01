using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class BangCanDoiKeToanDal : BaseDal<FastADOProvider>
    {
        public List<BangCanDoiKeToan> GetBangCanDoiKeToans(DateTime? dateTime, int buTru, string maDvcs)
        {
            try
            {
                return UnitOfWork.Procedure<BangCanDoiKeToan>("[dbo].[Gltcb]", new
                {
                    Ngay_ct = dateTime,
                    Bu_tru = buTru,
                    Mau = "GLTCB48",
                    Ma_dvcs = maDvcs
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<BangCanDoiKeToan>();
            }
        }
    }
}