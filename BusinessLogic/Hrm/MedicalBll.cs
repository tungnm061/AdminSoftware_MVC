using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class MedicalBll
    {
        private readonly MedicalDal _medicalDal;

        public MedicalBll()
        {
            _medicalDal = SingletonIpl.GetInstance<MedicalDal>();
        }

        public List<Medical> GetMedicals()
        {
            return _medicalDal.GetMedicals();
        }

        public Medical GetMedical(int medicalId)
        {
            return _medicalDal.GetMedical(medicalId);
        }

        public int Insert(Medical medical)
        {
            return _medicalDal.Insert(medical);
        }

        public bool Update(Medical medical)
        {
            return _medicalDal.Update(medical);
        }

        public bool Delete(int medicalId)
        {
            return _medicalDal.Delete(medicalId);
        }
    }
}