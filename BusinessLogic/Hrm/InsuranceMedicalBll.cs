using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class InsuranceMedicalBll
    {
        private readonly InsuranceMedicalDal _insuranceMedicalDal;

        public InsuranceMedicalBll()
        {
            _insuranceMedicalDal = SingletonIpl.GetInstance<InsuranceMedicalDal>();
        }

        public List<InsuranceMedical> GetInsuranceMedicals(int year, long? employeeId)
        {
            return _insuranceMedicalDal.GetInsuranceMedicals(year, employeeId);
        }

        public InsuranceMedical GetInsuranceMedical(string insuranceMedicalId)
        {
            return _insuranceMedicalDal.GetInsuranceMedical(insuranceMedicalId);
        }

        public bool Insert(InsuranceMedical insuranceMedical)
        {
            return _insuranceMedicalDal.Insert(insuranceMedical);
        }

        public bool Update(InsuranceMedical insuranceMedical)
        {
            return _insuranceMedicalDal.Update(insuranceMedical);
        }

        public bool Delete(string insuranceMedicalId)
        {
            return _insuranceMedicalDal.Delete(insuranceMedicalId);
        }
    }
}