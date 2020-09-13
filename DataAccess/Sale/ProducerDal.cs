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
    public class ProducerDal : BaseDal<ADOProvider>
    {
        public List<Producer> GetProducers()
        {
            try
            {
                return UnitOfWork.Procedure<Producer>("[sale].[Producer_GetAll]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Producer>();
            }
        }

        public Producer GetProducer(long id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Producer>("[sale].[Producer_GetById]", new { ProducerId = id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Producer obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProducerId", obj.ProducerId, DbType.Int64, ParameterDirection.Output);
                param.Add("@ProducerCode", obj.ProducerCode);
                param.Add("@ProducerName", obj.ProducerName);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                if (UnitOfWork.ProcedureExecute("[sale].[Producer_Insert]", param))
                {
                    return param.Get<long>("@ProducerId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Producer obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ProducerId", obj.ProducerId);
                param.Add("@ProducerName", obj.ProducerName);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                return UnitOfWork.ProcedureExecute("[sale].[Producer_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Producer_Delete]", new
                {
                    ProducerId = id
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
