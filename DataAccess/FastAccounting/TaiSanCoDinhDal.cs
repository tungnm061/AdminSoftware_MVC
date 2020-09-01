using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class TaiSanCoDinhDal : BaseDal<FastADOProvider>
    {
        public List<TaiSanCoDinh> GetTaiSanCoDinh(int year, int month, int firstOrLast, string maBpts,
            string loaiTs, string maDvcs)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[dbo].[FABCNV]", new
                {
                    Nam = year,
                    Ky = month,
                    Dau_Cuoi = firstOrLast,
                    Ma_bpts = maBpts,
                    Advance = " 1=1 AND loai_ts like '" + loaiTs + "%' AND ma_dvcs like '" + maDvcs + "%'"
                });
                var firstResult = multi.Read<FirstResult>().FirstOrDefault();
                if (firstResult != null)
                {
                    return multi.Read<TaiSanCoDinh>().ToList();
                }
                return new List<TaiSanCoDinh>();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TaiSanCoDinh>();
            }
        }
    }
}