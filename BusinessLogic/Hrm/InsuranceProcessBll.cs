using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class InsuranceProcessBll
    {
        private readonly InsuranceProcessDal _insuranceProcessDal;

        public InsuranceProcessBll()
        {
            _insuranceProcessDal = SingletonIpl.GetInstance<InsuranceProcessDal>();
        }

        public List<InsuranceProcess> GetInsuranceProcesses(long employeeId)
        {
            return _insuranceProcessDal.GetInsuranceProcesses(employeeId);
        }

        public InsuranceProcess GetInsuranceProcess(string insuranceProcessId)
        {
            return _insuranceProcessDal.GetInsuranceProcess(insuranceProcessId);
        }

        public bool Insert(InsuranceProcess insuranceProcess)
        {
            return _insuranceProcessDal.Insert(insuranceProcess);
        }

        public bool Update(InsuranceProcess insuranceProcess)
        {
            return _insuranceProcessDal.Update(insuranceProcess);
        }

        public bool Delete(string insuranceProcessId)
        {
            return _insuranceProcessDal.Delete(insuranceProcessId);
        }
    }
}