using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class WorkStreamDetailBll
    {
        private readonly WorkStreamDetailDal _workStreamDetailDal;

        public WorkStreamDetailBll()
        {
            _workStreamDetailDal = SingletonIpl.GetInstance<WorkStreamDetailDal>();
        }

        public WorkStreamDetail GetWorkStreamDetail(string workStreamDetailId)
        {
            return _workStreamDetailDal.GetWorkStreamDetail(workStreamDetailId);
        }

        public bool Update(WorkStreamDetail workStreamDetail)
        {
            return _workStreamDetailDal.Update(workStreamDetail);
        }
    }
}