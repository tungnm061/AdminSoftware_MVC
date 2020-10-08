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
   public class GmailOrderDetailDal : BaseDal<ADOProvider>
    {
        public bool Inserts(List<GmailOrderDetail> listObj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@XML", XmlHelper.SerializeXml<List<GmailOrderDetail>>(listObj));
                return UnitOfWork.ProcedureExecute("[sale].[GmailOrderDetail_Inserts]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<GmailOrderDetail> GetGmailOrderDetailByDate(string gmailOrderId,DateTime orderDate)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailOrderDetail>("[sale].[GmailOrderDetail_GetByDate]", new
                    {
                        GmailOrderId = gmailOrderId,
                        OrderDate = orderDate.ToString("MM/yyyy")
                    }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<GmailOrderDetail>();
            }
        }
    }
}
