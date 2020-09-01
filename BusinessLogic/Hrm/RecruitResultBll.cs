using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class RecruitResultBll
    {
        private readonly RecruitResultDal _recruitResultDal;

        public RecruitResultBll()
        {
            _recruitResultDal = SingletonIpl.GetInstance<RecruitResultDal>();
        }

        public List<RecruitResult> GetRecruitResults(long recruitPlanId)
        {
            return _recruitResultDal.GetRecruitResults(recruitPlanId);
        }

        public RecruitResult GetRecruitResult(string recruitResultId)
        {
            return _recruitResultDal.GetRecruitResult(recruitResultId);
        }

        public bool Insert(RecruitResult recruitResult)
        {
            return _recruitResultDal.Insert(recruitResult);
        }

        public bool Update(RecruitResult recruitResult)
        {
            return _recruitResultDal.Update(recruitResult);
        }

        public bool Delete(string recruitResultId)
        {
            return _recruitResultDal.Delete(recruitResultId);
        }
    }
}