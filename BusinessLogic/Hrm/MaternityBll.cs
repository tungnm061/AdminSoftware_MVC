using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class MaternityBll
    {
        private readonly MaternityDal _maternityDal;

        public MaternityBll()
        {
            _maternityDal = SingletonIpl.GetInstance<MaternityDal>();
        }

        public List<Maternity> GetMaternitys()
        {
            return _maternityDal.GetMaternitys();
        }

        public Maternity GetMaternity(string maternityId)
        {
            return _maternityDal.GetMaternity(maternityId);
        }

        public bool Insert(Maternity maternity)
        {
            return _maternityDal.Insert(maternity);
        }

        public bool Update(Maternity maternity)
        {
            return _maternityDal.Update(maternity);
        }

        public bool Delete(string maternityId)
        {
            return _maternityDal.Delete(maternityId);
        }
    }
}