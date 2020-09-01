using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class InsuranceBll
    {
        private readonly InsuranceDal _insuranceDal;

        public InsuranceBll()
        {
            _insuranceDal = SingletonIpl.GetInstance<InsuranceDal>();
        }

        public List<Insurance> GetInsurances(bool? isActive)
        {
            return _insuranceDal.GetInsurances(isActive);
        }

        public Insurance GetInsurance(long insuranceId)
        {
            return _insuranceDal.GetInsurance(insuranceId);
        }

        public Insurance GetInsuranceByEmployeeId(long employeeId)
        {
            return _insuranceDal.GetInsuranceByEmployeeId(employeeId);
        }

        public long Insert(Insurance insurance)
        {
            return _insuranceDal.Insert(insurance);
        }

        public bool Update(Insurance insurance)
        {
            return _insuranceDal.Update(insurance);
        }

        public bool Delete(long insuranceId)
        {
            return _insuranceDal.Delete(insuranceId);
        }
    }
}