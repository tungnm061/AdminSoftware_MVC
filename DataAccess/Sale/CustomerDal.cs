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
    public class CustomerDal : BaseDal<ADOProvider>
    {
        public List<Customer> GetCustomers(string keyword, int? status)
        {
            try
            {
                return UnitOfWork.Procedure<Customer>("[sale].[Get_Customers]", new
                {
                    Keyword = keyword,
                    Status = status
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Customer>();
            }
        }

        public Customer GetCustomer(long customerId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Customer>("[sale].[Get_Customer]", new {CustomerId = customerId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Customer customer)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CustomerId", customer.CustomerId, DbType.Int64, ParameterDirection.Output);
                param.Add("@Fullname", customer.FullName);
                param.Add("@CustomerCode", customer.CustomerCode);
                param.Add("@IdentityCardDate", customer.IdentityCardDate);
                param.Add("@CityIdentityCard", customer.CityIdentityCard);
                param.Add("@IdentityCard", customer.IdentityCard);
                param.Add("@Email", customer.Email);
                param.Add("@PhoneNumber", customer.PhoneNumber);
                param.Add("@Description", customer.Description);
                param.Add("@Address", customer.Address);
                param.Add("@CityId", customer.CityId);
                param.Add("@DistrictId", customer.DistrictId);
                param.Add("@TaxCode", customer.TaxCode);
                param.Add("@CompanyName", customer.CompanyName);
                param.Add("@BankAccountNumber", customer.BankAccountNumber);
                param.Add("@Status", customer.Status);
                param.Add("@CreateDate", customer.CreateDate);
                param.Add("@CreateBy", customer.CreateBy);
                if (UnitOfWork.ProcedureExecute("[sale].[Insert_Customer]", param))
                    return param.Get<long>("@CustomerId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Customer customer)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CustomerId", customer.CustomerId);
                param.Add("@Fullname", customer.FullName);
                param.Add("@CustomerCode", customer.CustomerCode);
                param.Add("@IdentityCardDate", customer.IdentityCardDate);
                param.Add("@CityIdentityCard", customer.CityIdentityCard);
                param.Add("@IdentityCard", customer.IdentityCard);
                param.Add("@Email", customer.Email);
                param.Add("@PhoneNumber", customer.PhoneNumber);
                param.Add("@Description", customer.Description);
                param.Add("@Address", customer.Address);
                param.Add("@CityId", customer.CityId);
                param.Add("@DistrictId", customer.DistrictId);
                param.Add("@TaxCode", customer.TaxCode);
                param.Add("@CompanyName", customer.CompanyName);
                param.Add("@BankAccountNumber", customer.BankAccountNumber);
                param.Add("@Status", customer.Status);
                param.Add("@CreateDate", customer.CreateDate);
                param.Add("@CreateBy", customer.CreateBy);
                return UnitOfWork.ProcedureExecute("[sale].[Update_Customer]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long customerId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Delete_Customer]", new
                {
                    CustomerId = customerId
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