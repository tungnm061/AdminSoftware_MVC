using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class ContractDal : BaseDal<ADOProvider>
    {
        public List<Contract> GetContracts()
        {
            try
            {
                return UnitOfWork.Procedure<Contract>("[hrm].[Get_Contracts]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Contract>();
            }
        }

        public List<Contract> GetContractsByEmployeeId(long employeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
                return UnitOfWork.Procedure<Contract>("[hrm].[Get_Contract_ByEmployeeId]", param).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Contract>();
            }
        }

        public Contract GetContract(string contractId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractId", contractId);
                return UnitOfWork.Procedure<Contract>("[hrm].[Get_Contract]", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Contract GetContractByContractCode(string contractCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractCode", contractCode);
                return UnitOfWork.Procedure<Contract>("[hrm].[Get_Contract_ByContractCode]", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(Contract contract)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractCode", contract.ContractCode);
                param.Add("@ContractFile", contract.ContractFile);
                param.Add("@ContractOthorFile", contract.ContractOthorFile);
                param.Add("@ContractId", contract.ContractId);
                param.Add("@ContractTypeId", contract.ContractTypeId);
                param.Add("@CreateBy", contract.CreateBy);
                param.Add("@CreateDate", contract.CreateDate);
                param.Add("@Description", contract.Description);
                param.Add("@EmployeeId", contract.EmployeeId);
                param.Add("@EndDate", contract.EndDate);
                param.Add("@StartDate", contract.StartDate);
                return UnitOfWork.ProcedureExecute("[hrm].[Insert_Contract]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Contract contract)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractCode", contract.ContractCode);
                param.Add("@ContractFile", contract.ContractFile);
                param.Add("@ContractOthorFile", contract.ContractOthorFile);
                param.Add("@ContractId", contract.ContractId);
                param.Add("@ContractTypeId", contract.ContractTypeId);
                param.Add("@Description", contract.Description);
                param.Add("@EmployeeId", contract.EmployeeId);
                param.Add("@EndDate", contract.EndDate);
                param.Add("@StartDate", contract.StartDate);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_Contract]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string contractId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractId", contractId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_Contract]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}