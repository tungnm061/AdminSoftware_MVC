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
    public class ContractDal : BaseDal<ADOProvider>
    {
        public List<Contract> GetContracts(DateTime fromDate, DateTime toDate, int? status)
        {
            try
            {
                return UnitOfWork.Procedure<Contract>("[sale].[Get_Contracts]", new
                {
                    FromDate = fromDate,
                    ToDate = toDate,
                    Status = status
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Contract>();
            }
        }

        public Contract GetContract(long contractId)
        {
            try
            {
                //return UnitOfWork.Procedure<Contract>("[sale].[Get_Contract]", new { ContractId = contractId }).FirstOrDefault();
                var param = new DynamicParameters();
                param.Add("@ContractId", contractId);
                var multi = UnitOfWork.ProcedureQueryMulti("[sale].[Get_Contract]", param);
                var contract = multi.Read<Contract>().FirstOrDefault();
                if (contract != null)
                {
                    contract.ContractDetails = multi.Read<ContractDetail>().ToList();
                }
                return contract;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(Contract contract)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractId", contract.ContractId, DbType.Int64, ParameterDirection.Output);
                param.Add("@ContractCode", contract.ContractCode);
                param.Add("@CustomerId", contract.CustomerId);
                param.Add("@EmployeeId", contract.EmployeeId);
                param.Add("@TotalPrice", contract.TotalPrice);
                param.Add("@Status", contract.Status);
                param.Add("@Description", contract.Description);
                param.Add("@CreateDate", contract.CreateDate);
                param.Add("@CreateBy", contract.CreateBy);
                param.Add("@ContractNumber", contract.ContractNumber);

                if (UnitOfWork.ProcedureExecute("[sale].[Insert_Contract]", param))
                {
                    return param.Get<long>("@ContractId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(Contract contract)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractId", contract.ContractId);
                param.Add("@ContractCode", contract.ContractCode);
                param.Add("@CustomerId", contract.CustomerId);
                param.Add("@EmployeeId", contract.EmployeeId);
                param.Add("@TotalPrice", contract.TotalPrice);
                param.Add("@Status", contract.Status);
                param.Add("@Description", contract.Description);
                param.Add("@CreateDate", contract.CreateDate);
                param.Add("@CreateBy", contract.CreateBy);
                param.Add("@ContractNumber", contract.ContractNumber);
                return UnitOfWork.ProcedureExecute("[sale].[Update_Contract]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long contractId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Delete_Contract]", new
                {
                    ContractId = contractId
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