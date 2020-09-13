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
    public class OrderBll
    {
        private readonly OrderDal _orderDal;
        private readonly OrderDetailDal _orderDetailDal;

        public OrderBll()
        {
            _orderDal = SingletonIpl.GetInstance<OrderDal>();
            _orderDetailDal = SingletonIpl.GetInstance<OrderDetailDal>();
        }


        public List<Order> GetOrders(int? keySearch = null, int? statusSearch = 0, string keyWord = "", DateTime? fromDate = null, DateTime? toDate = null, bool isActive = true,int? gmailId = null)
        {
            return _orderDal.GetOrders(keySearch, statusSearch, keyWord, fromDate, toDate, isActive, gmailId);
        }


        public Order GetOrder(long orderId)
        {
            return _orderDal.GetOrder(orderId);
        }

        public bool Delete(long id)
        {
            return _orderDal.Delete(id);
        }

        public bool Insert(Order obj)
        {
            try
            {
                if (obj == null || !obj.OrderDetails.Any())
                {
                    return false;
                }

                using (var scope = new TransactionScope())
                {
                    long orderId = _orderDal.Insert(obj);
                    if (orderId > 0)
                    {
                        if (_orderDetailDal.Insert(orderId, obj.OrderDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Order Order)
        {
            try
            {
                if (Order == null || !Order.OrderDetails.Any())
                {
                    return false;
                }
                var OrderOld = _orderDal.GetOrder(Order.OrderId);
                if (OrderOld == null)
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_orderDal.Update(Order))
                    {
                        if (!_orderDetailDal.Insert(Order.OrderId, Order.OrderDetails))
                        {
                            scope.Dispose();
                            return false;
                        }
                    }
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
