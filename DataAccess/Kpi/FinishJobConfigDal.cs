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
    public class FinishJobConfigDal : BaseDal<ADOProvider>
    {
        public List<FinishJobConfig> GetFinishJobConfigs()
        {
            try
            {
                return UnitOfWork.Procedure<FinishJobConfig>("[kpi].[Get_FinishJobConfigs]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<FinishJobConfig>();
            }
        }

        public FinishJobConfig GetFinishJobConfigCheck(int finishConfigId, decimal? conditionMin, decimal? conditionMax)
        {
            try
            {
                return UnitOfWork.Procedure<FinishJobConfig>("[kpi].[Get_FinishJobConfigCheck]", new
                {
                    FinishConfigId = finishConfigId,
                    ConditionMin = conditionMin,
                    ConditionMax = conditionMax
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new FinishJobConfig();
            }
        }

        public bool Update(FinishJobConfig finishConfig)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@FinishConfigId", finishConfig.FinishConfigId);
                param.Add("@FinishType", finishConfig.FinishType);
                param.Add("@FinishPointMin", finishConfig.FinishPointMin);
                param.Add("@FinishPointMax", finishConfig.FinishPointMax);
                param.Add("@FinishConditionMin", finishConfig.FinishConditionMin);
                param.Add("@FinishConditionMax", finishConfig.FinishConditionMax);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_FinishJobConfig]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}