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


        public List<Order> GetOrders(int? keySearch, int? statusSearch, string keyWord, DateTime? fromDate, DateTime? toDate, bool isActive,int? gmailId)
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

        public long Insert(Order obj)
        {
            try
            {
                if (obj == null || !obj.OrderDetails.Any())
                {
                    return 0;
                }

                using (var scope = new TransactionScope())
                {
                    long orderId = _orderDal.Insert(obj);
                    if (orderId > 0)
                    {
                        if (_orderDetailDal.Insert(orderId, obj.OrderDetails))
                        {
                            scope.Complete();
                            return orderId;
                        }
                        scope.Dispose();
                        return 0;
                    }
                    scope.Dispose();
                    return orderId;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public int Update(Order Order)
        {
            try
            {
                if (Order == null || !Order.OrderDetails.Any())
                {
                    return 0;
                }
                var obj = _orderDal.GetOrder(Order.OrderId);
                if (obj == null)
                {
                    return 0;
                }
                using (var scope = new TransactionScope())
                {
                    int update = _orderDal.Update(Order);
                    if (update > 0)
                    {
                        if (_orderDetailDal.Insert(Order.OrderId, Order.OrderDetails))
                        {
                            scope.Complete();
                            return 1;
                        }
                        scope.Dispose();
                        return 0;
                    }
                    scope.Dispose();
                    return update;
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
