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
    public class OrderDal : BaseDal<ADOProvider>
    {
        public List<Order> GetOrders(int? keySearch = null, int? statusSearch = 0,string keyWord ="",DateTime? fromDate = null,DateTime? toDate =null, bool isActive = true, int? gmailId = null)
        {
            try
            {
                return UnitOfWork.Procedure<Order>("[sale].[Order_GetAll]",new
                {
                    KeySerch = keySearch,
                    StatusSearch = statusSearch,
                    FromDate = fromDate,
                    ToDate = toDate,
                    Keyword = keyWord,
                    IsActive = isActive,
                    GmailId = gmailId

                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Order>();
            }
        }

        public Order GetOrder(long orderId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrderId", orderId);
                var multi = UnitOfWork.ProcedureQueryMulti("[sale].[Order_GetById]", param);
                var order = multi.Read<Order>().FirstOrDefault();
                if (order != null)
                {
                    order.OrderDetails = multi.Read<OrderDetail>().ToList();
                }
                return order;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Order obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrderId", obj.OrderId, DbType.Int64, ParameterDirection.Output);
                param.Add("@OrderCode", obj.OrderCode);
                param.Add("@FirstName", obj.FirstName);
                param.Add("@LastName", obj.LastName);
                param.Add("@Email", obj.Email);
                param.Add("@Phone", obj.Phone);
                param.Add("@CountryId", obj.CountryId);
                param.Add("@City", obj.City);
                param.Add("@Address1", obj.Address1);
                param.Add("@Address2", obj.Address2);
                param.Add("@Region", obj.Region);
                param.Add("@PostalZipCode", obj.PostalZipCode);
                param.Add("@Description", obj.Description);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@Status", obj.Status);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@TotalPrince", obj.TotalPrince);
                param.Add("@ProducerId", obj.ProducerId);
                param.Add("@TypeMoney", obj.TypeMoney);
                param.Add("@ShipMoney", obj.ShipMoney);
                param.Add("@FinishDate", obj.FinishDate);
                param.Add("@RateMoney", obj.RateMoney);
                param.Add("@RateMoney", obj.RateMoney);
                param.Add("@StartDate", obj.StartDate);
                param.Add("@TrackingCode", obj.TrackingCode);

                if (UnitOfWork.ProcedureExecute("[sale].[Order_Insert]", param))
                    return param.Get<long>("@OrderId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Order obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@OrderId", obj.OrderId);
                param.Add("@FirstName", obj.FirstName);
                param.Add("@LastName", obj.LastName);
                param.Add("@Email", obj.Email);
                param.Add("@Phone", obj.Phone);
                param.Add("@CountryId", obj.CountryId);
                param.Add("@City", obj.City);
                param.Add("@Address1", obj.Address1);
                param.Add("@Address2", obj.Address2);
                param.Add("@Region", obj.Region);
                param.Add("@PostalZipCode", obj.PostalZipCode);
                param.Add("@Description", obj.Description);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@Status", obj.Status);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@TotalPrince", obj.TotalPrince);
                param.Add("@ProducerId", obj.ProducerId);
                param.Add("@TypeMoney", obj.TypeMoney);
                param.Add("@ShipMoney", obj.ShipMoney);
                param.Add("@FinishDate", obj.FinishDate);
                param.Add("@RateMoney", obj.RateMoney);
                param.Add("@StartDate", obj.StartDate);
                param.Add("@TrackingCode", obj.TrackingCode);
                return UnitOfWork.ProcedureExecute("[sale].[Order_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long orderId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Order_Delete]", new
                {
                    OrderId = orderId
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
