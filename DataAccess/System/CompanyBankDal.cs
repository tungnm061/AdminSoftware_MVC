﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Common;
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
                param.Add("@ExpenseId", obj.ExpenseId);
                param.Add("@TypeMonney", obj.TypeMonney);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@TradingBy", obj.TradingBy);
                param.Add("@ExpenseText", obj.ExpenseText);
                param.Add("@CreateDate", obj.CreateDate);
                param.Add("@CreateBy", obj.CreateBy);
                param.Add("@IsActive", obj.IsActive);
                param.Add("@Status", obj.Status);
                param.Add("@FilePath", obj.FilePath);
                param.Add("@TextNote", obj.TextNote);
                if (UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Insert]", param))
                    return param.Get<int>("@CompanyBankId");
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
                param.Add("@ExpenseId", obj.ExpenseId);
                param.Add("@TypeMonney", obj.TypeMonney);
                param.Add("@MoneyNumber", obj.MoneyNumber);
                param.Add("@TradingDate", obj.TradingDate);
                param.Add("@TradingBy", obj.TradingBy);
                param.Add("@ExpenseText", obj.ExpenseText);
                param.Add("@UpdateDate", obj.UpdateDate);
                param.Add("@UpdateBy", obj.UpdateBy);
                param.Add("@Status", obj.Status);
                param.Add("@TextNote", obj.TextNote);
                param.Add("@FilePath", obj.FilePath);

                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Update]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool UpdateConfirm(CompanyBank obj)
        {

            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyBankId", obj.CompanyBankId);
                param.Add("@ConfirmBy", obj.ConfirmBy);
                param.Add("@ConfirmDate", obj.ConfirmDate);
                param.Add("@Status", obj.Status);
                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_UpdateConfirm]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }

        }

        public bool UpdateConfirmDetail(CompanyBank obj)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CompanyBankId", obj.CompanyBankId);
                param.Add("@ConfirmBy", obj.ConfirmBy);
                param.Add("@ConfirmDate", obj.ConfirmDate);
                param.Add("@Status", obj.Status);
                param.Add("@TextNote", obj.TextNote);

                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_UpdateConfirmDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }

        }

        public List<CompanyBank> GetCompanyBanks(bool? isActive,DateTime? fromDate,DateTime? toDate,int? expenseId, int? statusSearch)
        {
            try
            {
                return
                    UnitOfWork.Procedure<CompanyBank>("[dbo].[CompanyBank_GetAll]", 
                            new
                            {
                                IsActive = isActive ,
                                FromDate = fromDate,
                                ToDate = toDate,
                                ExpenseId = expenseId,
                                Status = statusSearch
                            })
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
                    UnitOfWork.Procedure<CompanyBank>("[dbo].[CompanyBank_GetById]", new { CompanyBankId = id })
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
                return UnitOfWork.ProcedureExecute("[dbo].[CompanyBank_Delete]", new { CompanyBankId = id });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

    }
}
