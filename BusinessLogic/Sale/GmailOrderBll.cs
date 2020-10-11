using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Sale;
using System.Linq;
using Entity.Sale;
using System.Transactions;
using System;
using Core.Helper.Logging;

namespace BusinessLogic.Sale
{
   public class GmailOrderBll
    {
        private readonly GmailOrderDal _gmailOrderDal;
        private readonly GmailOrderDetailDal _gmailOrderDetailDal;

        public GmailOrderBll()
        {
            _gmailOrderDal = SingletonIpl.GetInstance<GmailOrderDal>();
            _gmailOrderDetailDal = SingletonIpl.GetInstance<GmailOrderDetailDal>();
        }

        //public long Insert(List<GmailOrder> listGmailOrder, List<GmailOrderDetail> listGmailOrderDetail)
        //{
        //    try
        //    {
        //        if (listGmailOrder == null || listGmailOrder.Count == 0 ||
        //            listGmailOrderDetail == null || listGmailOrderDetail.Count == 0)
        //        {
        //            return 0;
        //        }

        //        using (var scope = new TransactionScope())
        //        {
        //            if (_gmailOrderDal.Inserts(listGmailOrder))
        //            {
        //                if (_gmailOrderDetailDal.Inserts(listGmailOrderDetail))
        //                {
        //                    scope.Complete();
        //                    return 1;
        //                }
        //                scope.Dispose();
        //                return 0;
        //            }
        //            scope.Dispose();
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.PutError(ex.Message, ex);
        //        return 0;
        //    }
        //}

        public List<GmailOrderModel> GetGmailOrderAll(int? gmailId, DateTime? fromDate, DateTime? toDate)
        {
            return _gmailOrderDal.GetGmailOrderAll(gmailId, fromDate, toDate);
        }

        public List<int> GetGmailIdDistinct(DateTime fromDate, DateTime toDate)
        {
            return _gmailOrderDal.GetGmailIdDistinct(fromDate, toDate);
        }

        public List<GmailOrderDetail> GetGmailOrderDetailByDate(string gmailOrderId, DateTime dateOrder)
        {
            return _gmailOrderDetailDal.GetGmailOrderDetailByDate(gmailOrderId, dateOrder);
        }

        public long Insert(List<GmailOrder> listGmailOrder)
        {
            try
            {
                if (listGmailOrder == null || listGmailOrder.Count == 0 )
                {
                    return 0;
                }

                using (var scope = new TransactionScope())
                {
                    if (_gmailOrderDal.Inserts(listGmailOrder))
                    {
                        scope.Complete();
                        return 1;
                    }
                    scope.Dispose();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }


    }
}
