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
                param.Add("@ExpenseName", obj.ExpenseName);
                param.Add("@Description", obj.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Insert]", param))
                    return param.Get<int>("@ExpenseId");
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
                param.Add("@ExpenseId", obj.ExpenseId);
                param.Add("@ExpenseName", obj.ExpenseName);
                param.Add("@Description", obj.Description);
                return UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<ExpenseType> GetExpenseTypes(bool? isActive)
        {
            try
            {
                return
                    UnitOfWork.Procedure<ExpenseType>("[dbo].[ExpenseType_GetAll]", new { IsActive = isActive })
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
                    UnitOfWork.Procedure<ExpenseType>("[dbo].[ExpenseType_GetById]", new { ExpenseId = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[ExpenseType_Delete]", new { ExpenseId = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}
