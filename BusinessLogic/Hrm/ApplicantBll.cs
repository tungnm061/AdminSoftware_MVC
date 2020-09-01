using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class ApplicantBll
    {
        private readonly ApplicantDal _applicantDal;

        public ApplicantBll()
        {
            _applicantDal = SingletonIpl.GetInstance<ApplicantDal>();
        }

        public List<Applicant> GetApplicants(long recruitPlanId)
        {
            return _applicantDal.GetApplicants(recruitPlanId);
        }

        public Applicant GetApplicant(string applicantId)
        {
            return _applicantDal.GetApplicant(applicantId);
        }

        public bool Insert(Applicant applicant)
        {
            return _applicantDal.Insert(applicant);
        }

        public bool Update(Applicant applicant)
        {
            return _applicantDal.Update(applicant);
        }

        public bool Delete(string applicantId)
        {
            return _applicantDal.Delete(applicantId);
        }
    }
}