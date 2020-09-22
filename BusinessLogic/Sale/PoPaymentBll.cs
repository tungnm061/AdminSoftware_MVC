using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

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

        public bool Delete(long id)
        {
            return _poPaymentDal.Delete(id);
        }
    }
}
