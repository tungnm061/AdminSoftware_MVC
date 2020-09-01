using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class SchoolBll
    {
        private readonly SchoolDal _nationDal;

        public SchoolBll()
        {
            _nationDal = SingletonIpl.GetInstance<SchoolDal>();
        }

        public List<School> GetSchools()
        {
            return _nationDal.GetSchools();
        }

        public School GetSchool(int nationId)
        {
            return _nationDal.GetSchool(nationId);
        }

        public int Insert(School nation)
        {
            return _nationDal.Insert(nation);
        }

        public bool Update(School nation)
        {
            return _nationDal.Update(nation);
        }

        public bool Delete(int nationId)
        {
            return _nationDal.Delete(nationId);
        }
    }
}