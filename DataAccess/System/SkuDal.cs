using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;
using Entity.System;

namespace DataAccess.System
{
    public class SkuDal : BaseDal<ADOProvider>
    {
        public int Insert(Sku obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@Code", obj.Code);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@Description", obj.Description);
                param.Add("@GmailId", obj.GmailId);
                if (UnitOfWork.ProcedureExecute("[dbo].[Sku_Insert]", param))
                    return param.Get<int>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Sku obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", obj.Id);
                param.Add("@Code", obj.Code);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@Description", obj.Description);

                return UnitOfWork.ProcedureExecute("[dbo].[Sku_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<Sku> GetSkus(int gmailId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Sku>("[dbo].[Sku_GetByGmailId]", new { GmailId = gmailId })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Sku>();
            }
        }

        public Sku GetSku(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Sku>("[dbo].[Sku_GetById]", new { Id = id })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[dbo].[Sku_Delete]", new { Id = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
