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
    public class GmailDal : BaseDal<ADOProvider>
    {
        public int Insert(Gmail gmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", gmail.Id, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@FullName", gmail.FullName);
                param.Add("@UserId", gmail.UserId);
                param.Add("@CreateDate", gmail.CreateDate);
                param.Add("@CreateBy", gmail.CreateBy);
                param.Add("@IsActive", gmail.IsActive);
                param.Add("@Description", gmail.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[Gmail_Insert]", param))
                    return param.Get<int>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Gmail gmail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", gmail.Id);
                param.Add("@FullName", gmail.FullName);
                param.Add("@UserId", gmail.UserId);
                param.Add("@UpdateDate", gmail.UpdateDate);
                param.Add("@UpdateBy", gmail.UpdateBy);
                param.Add("@IsActive", gmail.IsActive);
                param.Add("@Description", gmail.Description);

                return UnitOfWork.ProcedureExecute("[dbo].[Gmail_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<Gmail> GetGmails()
        {
            try
            {
                return
                    UnitOfWork.Procedure<Gmail>("[dbo].[Gmail_GetAll]", new { })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Gmail>();
            }
        }

        public Gmail GetGmailByName(string gmailName)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Gmail>("[dbo].[Gmail_GetByName]", new { GmailName = gmailName }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Gmail GetGmail(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Gmail>("[dbo].[Gmail_GetById]", new { Id = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[Gmail_Delete]", new { Id = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
