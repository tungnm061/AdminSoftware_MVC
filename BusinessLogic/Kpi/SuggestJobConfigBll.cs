using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class SuggestJobConfigBll
    {
        private readonly SuggestJobConfigDal _jobConfigDal;

        public SuggestJobConfigBll()
        {
            _jobConfigDal = SingletonIpl.GetInstance<SuggestJobConfigDal>();
        }

        public List<SuggetsJobConfig> GetJobConfigs()
        {
            return _jobConfigDal.GetSuggetsJobConfigs();
        }

        public SuggetsJobConfig GetSuggestJobCheckPoint(int jobConfigId, decimal? conditionMin, decimal? conditionMax)
        {
            return _jobConfigDal.GetSuggestJobCheckPoint(jobConfigId, conditionMin, conditionMax);
        }

        public bool Update(SuggetsJobConfig jobConfig)
        {
            return _jobConfigDal.Update(jobConfig);
        }
    }
}