using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class FactorConfigDal : BaseDal<ADOProvider>
    {
        public List<FactorConfig> GetFactorConfigs()
        {
            try
            {
                return UnitOfWork.Procedure<FactorConfig>("[kpi].[Get_FactorConfigs]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<FactorConfig>();
            }
        }

        public FactorConfig GetFactorConfigCheck(int factorConfigId, decimal? conditionMin, decimal? conditionMax)
        {
            try
            {
                return UnitOfWork.Procedure<FactorConfig>("[kpi].[Get_FactorConfigCheck]", new
                {
                    FactorConfigId = factorConfigId,
                    ConditionMin = conditionMin,
                    ConditionMax = conditionMax
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new FactorConfig();
            }
        }

        public bool Update(FactorConfig factorConfig)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@FactorConfigId", factorConfig.FactorConfigId);
                param.Add("@FactorType", factorConfig.FactorType);
                param.Add("@FactorPointMin", factorConfig.FactorPointMin);
                param.Add("@FactorPointMax", factorConfig.FactorPointMax);
                param.Add("@FactorConditionMin", factorConfig.FactorConditionMin);
                param.Add("@FactorConditionMax", factorConfig.FactorConditionMax);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_FactorConfig]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}