using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class KpiConfigBll
    {
        private readonly KpiConfigDal _kpiConfigDal;

        public KpiConfigBll()
        {
            _kpiConfigDal = SingletonIpl.GetInstance<KpiConfigDal>();
        }

        public KpiConfig GetKpiConfig()
        {
            return _kpiConfigDal.GetKpiConfig();
        }

        public bool Update(KpiConfig kpiConfig)
        {
            return _kpiConfigDal.Update(kpiConfig);
        }
    }
}