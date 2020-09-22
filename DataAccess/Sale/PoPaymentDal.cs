using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;
namespace DataAccess.Sale
{
    public class PoPaymentDal : BaseDal<ADOProvider>
    {

        public List<PoPayment> GetPoPayments(int ?status, DateTime? fromDate,DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<PoPayment>("[sale].[PoPayment_GetAll]", new
                {
                    Status = status,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<PoPayment>();
            }
        }


        public PoPayment GetPoPayment(long id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<PoPayment>("[sale].[PoPayment_GetById]", new { PoPaymentId = id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(PoPayment obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PoPaymentId", obj.PoPaymentId, DbType.Int64, ParameterDirection.Output);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@RateMoney", obj.RateMoney);
                param.Add("@TypeMoney", obj.TypeMoney);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@Status", obj.Status);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                if (UnitOfWork.ProcedureExecute("[sale].[PoPayment_Insert]", param))
                {
                    return param.Get<long>("@PoPaymentId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(PoPayment obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PoPaymentId", obj.PoPaymentId);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@RateMoney", obj.RateMoney);
                param.Add("@TypeMoney", obj.TypeMoney);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@Status", obj.Status);
                param.Add("@ConfirmDate", obj.ConfirmDate);
                param.Add("@ConfirmBy", obj.ConfirmBy);
                return UnitOfWork.ProcedureExecute("[sale].[PoPayment_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }


        public bool Delete(long id)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[PoPayment_Delete]", new
                {
                    PoPaymentId = id
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
