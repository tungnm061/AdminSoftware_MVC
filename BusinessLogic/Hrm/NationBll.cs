using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class NationBll
    {
        private readonly NationDal _nationDal;

        public NationBll()
        {
            _nationDal = SingletonIpl.GetInstance<NationDal>();
        }

        public List<Nation> GetNations()
        {
            return _nationDal.GetNations();
        }

        public Nation GetNation(int nationId)
        {
            return _nationDal.GetNation(nationId);
        }

        public int Insert(Nation nation)
        {
            return _nationDal.Insert(nation);
        }

        public bool Update(Nation nation)
        {
            return _nationDal.Update(nation);
        }

        public bool Delete(int nationId)
        {
            return _nationDal.Delete(nationId);
        }
    }
}