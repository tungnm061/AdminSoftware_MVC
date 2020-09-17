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
    public class ExpenseTypeDal : BaseDal<ADOProvider>
    {
        public int Insert(ExpenseType obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ExpenseId", obj.ExpenseId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@UserName", obj.ExpenseName);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Insert]", param))
                    return param.Get<int>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(ExpenseType obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Id", obj.ExpenseId);
                param.Add("@UserName", obj.ExpenseName);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                return UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<ExpenseType> GetExpenseTypes()
        {
            try
            {
                return
                    UnitOfWork.Procedure<ExpenseType>("[dbo].[ExpenseType_GetAll]", new { })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<ExpenseType>();
            }
        }

        public ExpenseType GetExpenseType(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<ExpenseType>("[dbo].[ExpenseType_GetById]", new { Id = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Delete]", new { Id = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}
