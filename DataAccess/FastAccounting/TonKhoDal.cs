using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Entity.FastAccounting;

namespace DataAccess.FastAccounting
{
    public class TonKhoDal : BaseDal<FastADOProvider>
    {
        public List<TonKho> TonKhos(string date, string filter, string condition)
        {
            try
            {
                return UnitOfWork.Procedure<TonKho>("[dbo].[INSD2CK]", new
                {
                    Date = date,
                    Filter = filter,
                    Condition = condition
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TonKho>();
            }
        }

        public List<TonKho> NhapXuatTons(string fromDate, string toDate, int tinhDc, string condition, string vtTonKho,
            string conditionSd)
        {
            try
            {
                return UnitOfWork.Procedure<TonKho>("[dbo].[INCD1]", new
                {
                    StartDate = fromDate,
                    EndDate = toDate,
                    Tinh_dc = tinhDc,
                    Condition = condition,
                    Vttonkho = vtTonKho,
                    ConditionSD = conditionSd
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<TonKho>();
            }
        }
    }
}