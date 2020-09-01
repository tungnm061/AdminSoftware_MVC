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
    public class SuggestJobConfigDal : BaseDal<ADOProvider>
    {
        public List<SuggetsJobConfig> GetSuggetsJobConfigs()
        {
            try
            {
                return UnitOfWork.Procedure<SuggetsJobConfig>("[kpi].[Get_SuggestJobConfigs]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<SuggetsJobConfig>();
            }
        }

        public SuggetsJobConfig GetSuggestJobCheckPoint(int jobConfigId, decimal? conditionMin, decimal? conditionMax)
        {
            try
            {
                return UnitOfWork.Procedure<SuggetsJobConfig>("[kpi].[Get_SuggestJobCheckPoint]", new
                {
                    JobConfigId = jobConfigId,
                    ConditionMin = conditionMin,
                    ConditionMax = conditionMax
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new SuggetsJobConfig();
            }
        }

        public bool Update(SuggetsJobConfig jobConfig)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@JobConfigId", jobConfig.JobConfigId);
                param.Add("@JobType", jobConfig.JobType);
                param.Add("@JobPointMin", jobConfig.JobPointMin);
                param.Add("@JobPointMax", jobConfig.JobPointMax);
                param.Add("@JobConditionMin", jobConfig.JobConditionMin);
                param.Add("@JobConditionMax", jobConfig.JobConditionMax);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_SuggestJobConfig]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}