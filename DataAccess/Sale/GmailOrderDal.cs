using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;

namespace DataAccess.Sale
{
   public class GmailOrderDal : BaseDal<ADOProvider>
    {
        public bool Inserts(List<GmailOrder> listObj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", XmlHelper.SerializeXml<List<GmailOrder>>(listObj));
                return UnitOfWork.ProcedureExecute("[sale].[GmailOrder_Inserts]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public GmailOrderModel GetGmailOrderByDate(DateTime orderDate)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailOrderModel>("[sale].[GmailOrder_GetByDate]", new
                    {
                        OrderDate = orderDate
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public List<GmailOrderModel> GetGmailOrderAll(int? gmailId, DateTime? fromDate,DateTime? toDate)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailOrderModel>("[sale].[GmailOrder_GetAll]", new
                    {
                        FromDate = fromDate,
                        ToDate = toDate,
                        GmailId = gmailId
                    }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<GmailOrderModel>();
            }
        }

        public List<int> GetGmailIdDistinct(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return
                    UnitOfWork.Procedure<int>("[sale].[GmailOrder_Distinct]", new {
                        FromDate = fromDate,
                        ToDate = toDate
                    }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<int>(); ;
            }
        }

        public bool DeleteByDate(DateTime orderDate)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[GmailOrder_DeleteByDate]", new
                {
                    OrderDate = orderDate
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
