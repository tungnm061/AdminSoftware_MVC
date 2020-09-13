using System;
using System.Collections.Generic;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;

namespace DataAccess.Sale
{
   public class OrderDetailDal : BaseDal<ADOProvider>
    {

       public bool Insert(long orderId, List<OrderDetail> orderDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrderId", orderId);
                param.Add("@XML", XmlHelper.SerializeXml<List<OrderDetail>>(orderDetails));
                return UnitOfWork.ProcedureExecute("[sale].[OrderDetail_Inserts]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}
