using System;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class RecruitPlanBll
    {
        private readonly RecruitPlanDal _recruitPlanDal;

        public RecruitPlanBll()
        {
            _recruitPlanDal = SingletonIpl.GetInstance<RecruitPlanDal>();
        }

        public List<RecruitPlan> GetRecruitPlans(bool? isActive, DateTime fromDate, DateTime toDate)
        {
            return _recruitPlanDal.GetRecruitPlans(isActive, fromDate, toDate);
        }

        public RecruitPlan GetRecruitPlan(long recruitPlanId)
        {
            return _recruitPlanDal.GetRecruitPlan(recruitPlanId);
        }

        public long Insert(RecruitPlan recruitPlan, ref string recruitPlanCode)
        {
            return _recruitPlanDal.Insert(recruitPlan, ref recruitPlanCode);
        }

        public bool Update(RecruitPlan recruitPlan)
        {
            return _recruitPlanDal.Update(recruitPlan);
        }

        public bool Delete(long recruitPlanId)
        {
            return _recruitPlanDal.Delete(recruitPlanId);
        }
    }
}