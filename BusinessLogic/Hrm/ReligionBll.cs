using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class ReligionBll
    {
        private readonly ReligionDal _religionDal;

        public ReligionBll()
        {
            _religionDal = SingletonIpl.GetInstance<ReligionDal>();
        }

        public List<Religion> GetReligions()
        {
            return _religionDal.GetReligions();
        }

        public Religion GetReligion(int religionId)
        {
            return _religionDal.GetReligion(religionId);
        }

        public int Insert(Religion religion)
        {
            return _religionDal.Insert(religion);
        }

        public bool Update(Religion religion)
        {
            return _religionDal.Update(religion);
        }

        public bool Delete(int religionId)
        {
            return _religionDal.Delete(religionId);
        }
    }
}