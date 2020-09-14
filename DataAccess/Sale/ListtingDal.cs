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
   public class ListtingDal : BaseDal<ADOProvider>
    {

        public List<Listting> GetListtings(bool isActive = true, string keyWord = "")
        {
            try
            {
                return UnitOfWork.Procedure<Listting>("[sale].[Listting_GetAll]", new
                {
                    IsActive = isActive,
                    Keyword = keyWord
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Listting>();
            }
        }

        public Listting GetListting(long id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Listting>("[sale].[Listting_GetById]", new { ListtingId = id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Listting obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ListtingId", obj.ListtingId, DbType.Int64, ParameterDirection.Output);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@ThreeNumberPayOnner", obj.ThreeNumberPayOnner);
                param.Add("@PayOnner", obj.PayOnner);
                param.Add("@ListProduct", obj.ListProduct);
                param.Add("@Balance", obj.Balance);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                if (UnitOfWork.ProcedureExecute("[sale].[Listting_Insert]", param))
                {
                    return param.Get<long>("@ListtingId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Listting obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ListtingId", obj.ListtingId);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@ThreeNumberPayOnner", obj.ThreeNumberPayOnner);
                param.Add("@PayOnner", obj.PayOnner);
                param.Add("@ListProduct", obj.ListProduct);
                param.Add("@Balance", obj.Balance);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                return UnitOfWork.ProcedureExecute("[sale].[Listting_Update]", param);
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
                return UnitOfWork.ProcedureExecute("[sale].[Listting_Delete]", new
                {
                    ListtingId = id
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
