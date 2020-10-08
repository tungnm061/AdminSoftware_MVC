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
    public class GmailRegDal : BaseDal<ADOProvider>
    {
        public int Insert(GmailReg obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@GmailRegId", obj.GmailRegId, DbType.Int32, ParameterDirection.Output);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@GmailRestoreId", obj.GmailRestoreId);
                param.Add("@Password", obj.Password);
                var insert = UnitOfWork.ProcedureExecute("[dbo].[GmailReg_Insert]", param);
                if (insert)
                {
                    return param.Get<int>("@GmailRegId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public int Update(GmailReg obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Result", null, DbType.Int32, ParameterDirection.Output);
                param.Add("@GmailRegId", obj.GmailRegId);
                param.Add("@GmailId", obj.GmailId);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                param.Add("@GmailRestoreId", obj.GmailRestoreId);
                param.Add("@Password", obj.Password);
                var update = UnitOfWork.ProcedureExecute("[dbo].[GmailReg_Update]", param);
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

        public List<GmailReg> GetGmailRegs(bool? isActive)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailReg>("[dbo].[GmailReg_GetAll]", new { IsActive = isActive })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<GmailReg>();
            }
        }


        public GmailReg GetGmailReg(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<GmailReg>("[dbo].[GmailReg_GetById]", new { GmailRegId = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[GmailReg_Delete]", new { GmailRegId = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
