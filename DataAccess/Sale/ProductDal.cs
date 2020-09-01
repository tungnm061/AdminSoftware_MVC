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
    public class ProductDal : BaseDal<ADOProvider>
    {
        public List<Product> GetProducts(bool? isActive, string keyword)
        {
            try
            {
                return UnitOfWork.Procedure<Product>("[sale].[Get_Products]", new
                {
                    IsActive = isActive,
                    Keyword = keyword
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Product>();
            }
        }

        public List<Product> GetProductsSell()
        {
            try
            {
                return UnitOfWork.Procedure<Product>("[sale].[Get_ContractsBySell]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Product>();
            }
        }

        public Product GetProduct(long productId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Product>("[sale].[Get_Product]", new {ProductId = productId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Product product)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", product.ProductId, DbType.Int64, ParameterDirection.Output);
                param.Add("@ProductCode", product.ProductCode);
                param.Add("@ProductName", product.ProductName);
                param.Add("@Price", product.Price);
                param.Add("@Quantity", product.Quantity);
                param.Add("@IsActive", product.IsActive);
                param.Add("@Description", product.Description);
                param.Add("@CreateDate", product.CreateDate);
                param.Add("@CreateBy", product.CreateBy);
                if (UnitOfWork.ProcedureExecute("[sale].[Insert_Product]", param))
                {
                    return param.Get<long>("@ProductId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Product product)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProductId", product.ProductId);
                param.Add("@ProductCode", product.ProductCode);
                param.Add("@ProductName", product.ProductName);
                param.Add("@Price", product.Price);
                param.Add("@Quantity", product.Quantity);
                param.Add("@IsActive", product.IsActive);
                param.Add("@Description", product.Description);
                param.Add("@CreateDate", product.CreateDate);
                param.Add("@CreateBy", product.CreateBy);
                return UnitOfWork.ProcedureExecute("[sale].[Update_Product]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long productId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Delete_Product]", new
                {
                    ProductId = productId
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