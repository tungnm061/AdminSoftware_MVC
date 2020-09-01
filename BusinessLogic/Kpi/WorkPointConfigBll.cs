using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class WorkPointConfigBll
    {
        private readonly WorkPointConfigDal _workPointConfigDal;

        public WorkPointConfigBll()
        {
            _workPointConfigDal = SingletonIpl.GetInstance<WorkPointConfigDal>();
        }

        public List<WorkPointConfig> GetWorkPointConfigs()
        {
            return _workPointConfigDal.GetWorkPointConfigs();
        }

        public WorkPointConfig GetWorkPointConfig(int workPointConfigId)
        {
            return _workPointConfigDal.GetWorkPointConfig(workPointConfigId);
        }
    }
}