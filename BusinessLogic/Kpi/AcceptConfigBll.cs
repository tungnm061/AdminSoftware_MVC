using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class AcceptConfigBll
    {
        private readonly AcceptConfigDal _acceptConfigDal;

        public AcceptConfigBll()
        {
            _acceptConfigDal = SingletonIpl.GetInstance<AcceptConfigDal>();
        }

        public AcceptConfig GetAcceptConfigCheck(int acceptConfigId,
            decimal? conditionMin, decimal? conditionMax)
        {
            return _acceptConfigDal.GetAcceptConfigCheck(acceptConfigId, conditionMin, conditionMax);
        }

        public List<AcceptConfig> GetAcceptConfigs()
        {
            return _acceptConfigDal.GetAcceptConfigs();
        }

        public bool Update(AcceptConfig acceptConfig)
        {
            return _acceptConfigDal.Update(acceptConfig);
        }
    }
}