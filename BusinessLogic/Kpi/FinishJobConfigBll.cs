using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class FinishJobConfigBll
    {
        private readonly FinishJobConfigDal _finishConfigDal;

        public FinishJobConfigBll()
        {
            _finishConfigDal = SingletonIpl.GetInstance<FinishJobConfigDal>();
        }

        public List<FinishJobConfig> GetFinishConfigs()
        {
            return _finishConfigDal.GetFinishJobConfigs();
        }

        public FinishJobConfig GetFinishJobConfigCheck(int finishConfigId,
            decimal? conditionMin, decimal? conditionMax)
        {
            return _finishConfigDal.GetFinishJobConfigCheck(finishConfigId, conditionMin,
                conditionMax);
        }

        public bool Update(FinishJobConfig finishConfig)
        {
            return _finishConfigDal.Update(finishConfig);
        }
    }
}