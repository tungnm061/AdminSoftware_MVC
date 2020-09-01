using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class WorkPlanDetailBll
    {
        private readonly SuggesWorkDal _suggesWorkDal;
        private readonly WorkPlanDetailDal _workPlanDetailDal;

        public WorkPlanDetailBll()
        {
            _workPlanDetailDal = SingletonIpl.GetInstance<WorkPlanDetailDal>();
            _suggesWorkDal = SingletonIpl.GetInstance<SuggesWorkDal>();
        }

        public WorkPlanDetail GetWorkPlanDetail(string workPlanDetailId)
        {
            return _workPlanDetailDal.GetWorkPlanDetail(workPlanDetailId);
        }

        public List<WorkPlanDetail> GetWorkPlanDetailsNeedCompleted(DateTime date)
        {
            return _workPlanDetailDal.GetWorkPlanDetailsNeedCompleted(date);
        }

        public bool Update(WorkPlanDetail workPlanDetail)
        {
            return _workPlanDetailDal.Update(workPlanDetail);
        }
    }
}