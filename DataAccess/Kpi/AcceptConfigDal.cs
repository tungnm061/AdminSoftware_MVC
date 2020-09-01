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
    public class AcceptConfigDal : BaseDal<ADOProvider>
    {
        public List<AcceptConfig> GetAcceptConfigs()
        {
            try
            {
                return UnitOfWork.Procedure<AcceptConfig>("[kpi].[Get_AcceptConfigs]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<AcceptConfig>();
            }
        }

        public AcceptConfig GetAcceptConfigCheck(int acceptConfigId, decimal? conditionMin, decimal? conditionMax)
        {
            try
            {
                return UnitOfWork.Procedure<AcceptConfig>("[kpi].[Get_AcceptConfigCheck]", new
                {
                    AcceptConfigId = acceptConfigId,
                    ConditionMin = conditionMin,
                    ConditionMax = conditionMax
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new AcceptConfig();
            }
        }

        public bool Update(AcceptConfig acceptConfig)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@AcceptConfigId", acceptConfig.AcceptConfigId);
                param.Add("@AcceptType", acceptConfig.AcceptType);
                param.Add("@AcceptPointMin", acceptConfig.AcceptPointMin);
                param.Add("@AcceptPointMax", acceptConfig.AcceptPointMax);
                param.Add("@AcceptConditionMin", acceptConfig.AcceptConditionMin);
                param.Add("@AcceptConditionMax", acceptConfig.AcceptConditionMax);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_AcceptConfig]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}