using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Sale;

namespace DataAccess.Sale
{
    public class ContractDetailDal : BaseDal<ADOProvider>
    {
        public ContractDetail GetContractDetail(string contractDetailId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<ContractDetail>("[sale].[Get_ContractDetail]",
                        new {ContractDetailId = contractDetailId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new ContractDetail();
            }
        }

        public bool Insert(long contractId, List<ContractDetail> listContractDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ContractId", contractId);
                param.Add("@XML", XmlHelper.SerializeXml<List<ContractDetail>>(listContractDetail));
                return UnitOfWork.ProcedureExecute("[sale].[Insert_ContractDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string contractDetailId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[sale].[Detele_ContractDetail]", new
                {
                    ContractDetailId = contractDetailId
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