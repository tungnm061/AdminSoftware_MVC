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
    public class CompanyBankDal : BaseDal<ADOProvider>
    {

        public int Insert(CompanyBank obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyBankId", obj.CompanyBankId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@ExpenseType", obj.ExpenseType);
                param.Add("@TypeMonney", obj.TypeMonney);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@TradingBy", obj.TradingBy);
                param.Add("@ExpenseText", obj.ExpenseText);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                if (UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Insert]", param))
                    return param.Get<int>("@Id");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(CompanyBank obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyBankId", obj.CompanyBankId);
                param.Add("@ExpenseType", obj.ExpenseType);
                param.Add("@TypeMonney", obj.TypeMonney);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@TradingBy", obj.TradingBy);
                param.Add("@ExpenseText", obj.ExpenseText);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Description", obj.Description);
                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public List<CompanyBank> GetCompanyBanks()
        {
            try
            {
                return
                    UnitOfWork.Procedure<CompanyBank>("[dbo].[CompanyBank_GetAll]", new { })
                        .ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<CompanyBank>();
            }
        }

        public CompanyBank GetCompanyBank(int id)
        {
            try
            {
                return
                    UnitOfWork.Procedure<CompanyBank>("[dbo].[CompanyBank_GetById]", new { Id = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Delete]", new { Id = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
