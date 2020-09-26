using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Transactions;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Common;
using Entity.Sale;
using Entity.System;

namespace BusinessLogic.Sale
{
    public class PoPaymentBll
    {
        private readonly PoPaymentDal _poPaymentDal;

        public PoPaymentBll()
        {
            _poPaymentDal = SingletonIpl.GetInstance<PoPaymentDal>();
        }

        public PoPayment GetPoPayment(long id)
        {
            return _poPaymentDal.GetPoPayment(id);
        }

        public List<PoPayment> GetPoPayments(int? status, DateTime? fromDate, DateTime? toDate)
        {
            return _poPaymentDal.GetPoPayments(status, fromDate, toDate);
        }

        public long Insert(PoPayment obj)
        {
            return _poPaymentDal.Insert(obj);
        }

        public bool Update(PoPayment obj)
        {
            return _poPaymentDal.Update(obj);
        }

        public bool UpdateConfirmDetail(PoPayment obj)
        {
            return _poPaymentDal.UpdateConfirmDetail(obj);
        }

        public BizResult UpdateConfirm(List<PoPayment> listObj)
        {
            BizResult rs = new BizResult();
            rs.Status = 1;
            using (var scope = new TransactionScope())
            {
                foreach (var item in listObj)
                {
                    if (!_poPaymentDal.UpdateConfirm(item))
                    {
                        rs.Status = -1;
                        break;
                    }
                }

                if (rs.Status == 1)
                {
                    scope.Complete();
                }
                else
                {
                    scope.Dispose();
                }

                return rs;
            }
        }
        public bool Delete(long id)
        {
            return _poPaymentDal.Delete(id);
        }
    }
}
