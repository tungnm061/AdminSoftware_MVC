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
    public class GmailRemoveDal : BaseDal<ADOProvider>
    {
        public int Insert(GmailRemove obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@GmailRemoveId", obj.GmailRemoveId, DbType.Int32, ParameterDirection.Output);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@GmailRestoreId", obj.GmailRestoreId);
                param.Add("@Password", obj.Password);
                param.Add("@GmailChangeId", obj.GmailChangeId);
                param.Add("@PasswordGmailChange", obj.PasswordGmailChange);
                param.Add("@GmailRestoreChangeId", obj.GmailRestoreChangeId);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                var insert = UnitOfWork.ProcedureExecute("[dbo].[GmailRemove_Insert]", param);
                if (insert)
                {
                    return param.Get<int>("@GmailRemoveId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public int Update(GmailRemove obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Result", null, DbType.Int32, ParameterDirection.Output);
                param.Add("@GmailRemoveId", obj.GmailRemoveId);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@GmailRestoreId", obj.GmailRestoreId);
                param.Add("@Password", obj.Password);
                param.Add("@GmailChangeId", obj.GmailChangeId);
                param.Add("@PasswordGmailChange", obj.PasswordGmailChange);
                param.Add("@GmailRestoreChangeId", obj.GmailRestoreChangeId);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                var update = UnitOfWork.ProcedureExecute("[dbo].[GmailRemove_Update]", param);
                if (update)
                {
                    return param.Get<int>("@Result");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public List<GmailRemove> GetGmailRemoves(bool? isActive)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailRemove>("[dbo].[GmailRemove_GetAll]", new { IsActive = isActive })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<GmailRemove>();
            }
        }


        public GmailRemove GetGmailRemove(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailRemove>("[dbo].[GmailRemove_GetById]", new { GmailRemoveId = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[GmailRemove_Delete]", new { GmailRemoveId = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
