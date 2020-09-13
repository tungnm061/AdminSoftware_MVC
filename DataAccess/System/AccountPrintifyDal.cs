using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.System;

namespace DataAccess.System
{
    public class AccountPrintifyDal : BaseDal<ADOProvider>
    {
        public int Insert(AccountPrintify acc)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", acc.Id, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@UserName", acc.UserName);
                param.Add("@Token", acc.Token);
                param.Add("@CreateDate", acc.CreateDate);
                param.Add("@CreateBy", acc.CreateBy);
                param.Add("@IsActive", acc.IsActive);
                param.Add("@Description", acc.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[AccountPrintify_Insert]", param))
                    return param.Get<int>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(AccountPrintify acc)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", acc.Id);
                param.Add("@UserName", acc.UserName);
                param.Add("@Token", acc.Token);
                param.Add("@UpdateDate", acc.UpdateDate);
                param.Add("@UpdateBy", acc.UpdateBy);
                param.Add("@IsActive", acc.IsActive);
                param.Add("@Description", acc.Description);
                return UnitOfWork.ProcedureExecute("[dbo].[AccountPrintify_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<AccountPrintify> GetAccountPrintifys()
        {
            try
            {
                return
                    UnitOfWork.Procedure<AccountPrintify>("[dbo].[AccountPrintify_GetAll]", new { })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<AccountPrintify>();
            }
        }

        public AccountPrintify GetAccountPrintify(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<AccountPrintify>("[dbo].[AccountPrintify_GetById]", new { Id = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[AccountPrintify_Delete]", new { Id = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
