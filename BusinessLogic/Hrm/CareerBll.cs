using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class CareerBll
    {
        private readonly CareerDal _careerDal;

        public CareerBll()
        {
            _careerDal = SingletonIpl.GetInstance<CareerDal>();
        }

        public List<Career> GetCareers()
        {
            return _careerDal.GetCareers();
        }

        public Career GetCareer(int careerId)
        {
            return _careerDal.GetCareer(careerId);
        }

        public int Insert(Career career)
        {
            return _careerDal.Insert(career);
        }

        public bool Update(Career career)
        {
            return _careerDal.Update(career);
        }

        public bool Delete(int careerId)
        {
            return _careerDal.Delete(careerId);
        }
    }
}