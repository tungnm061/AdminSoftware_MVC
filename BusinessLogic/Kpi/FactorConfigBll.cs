using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class FactorConfigBll
    {
        private readonly FactorConfigDal _factorConfigDal;

        public FactorConfigBll()
        {
            _factorConfigDal = SingletonIpl.GetInstance<FactorConfigDal>();
        }

        public List<FactorConfig> GetFactorConfigs()
        {
            return _factorConfigDal.GetFactorConfigs();
        }

        public FactorConfig GetFactorConfigCheck(int factorConfigId,
            decimal? conditionMin, decimal? conditionMax)
        {
            return _factorConfigDal.GetFactorConfigCheck(factorConfigId, conditionMin, conditionMax);
        }

        public bool Update(FactorConfig factorConfig)
        {
            return _factorConfigDal.Update(factorConfig);
        }
    }
}