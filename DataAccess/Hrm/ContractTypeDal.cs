using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class ContractTypeDal : BaseDal<ADOProvider>
    {
        public List<ContractType> GetContractTypes(bool? isActive)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IsActive", isActive);
                return UnitOfWork.Procedure<ContractType>("[hrm].[Get_ContractTypes]", param).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<ContractType>();
            }
        }

        public ContractType GetContractType(int contractTypeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractTypeId", contractTypeId);
                return UnitOfWork.Procedure<ContractType>("[hrm].[Get_ContractType]", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public int Insert(ContractType contractType)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractTypeId", contractType.ContractTypeId, DbType.Int32, ParameterDirection.Output);
                param.Add("@TypeName", contractType.TypeName);
                param.Add("@Description", contractType.Description);
                param.Add("@IsActive", contractType.IsActive);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_ContractType]", param))
                    return param.Get<int>("@ContractTypeId");
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(ContractType contractType)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractTypeId", contractType.ContractTypeId);
                param.Add("@TypeName", contractType.TypeName);
                param.Add("@Description", contractType.Description);
                param.Add("@IsActive", contractType.IsActive);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_ContractType]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(int contractTypeId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_ContractType]", new {ContractTypeId = contractTypeId});
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}