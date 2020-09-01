using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class CongCuDungCuDal : BaseDal<FastADOProvider>
    {
        public List<CongCuDungCu> GetCongCuDungCu(int year, int month, int firstOrLast, string maBpCc,
            string loaiCc, string maDvcs)
        {
            try
            {
                var multi = UnitOfWork.ProcedureQueryMulti("[dbo].[FXBCNV]", new
                {
                    Nam = year,
                    Ky = month,
                    Dau_Cuoi = firstOrLast,
                    Ma_BpCc = maBpCc,
                    Advance = " 1=1 AND loai_cc like '" + loaiCc + "%' AND ma_dvcs like '" + maDvcs + "%'"
                });
                var firstResult = multi.Read<FirstResult>().FirstOrDefault();
                if (firstResult != null)
                {
                    return multi.Read<CongCuDungCu>().ToList();
                }
                return new List<CongCuDungCu>();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<CongCuDungCu>();
            }
        }
    }
}