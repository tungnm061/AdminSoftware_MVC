using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class ListtingBll
    {
        private readonly ListtingDal _ListtingDal;

        public ListtingBll()
        {
            _ListtingDal = SingletonIpl.GetInstance<ListtingDal>();
        }


        public Listting GetListting(long id)
        {
            return _ListtingDal.GetListting(id);
        }

        public List<Listting> GetListtings(bool isActive = true, string keyWord = "")
        {
            return _ListtingDal.GetListtings(isActive, keyWord);
        }

        public long Insert(Listting obj)
        {
            return _ListtingDal.Insert(obj);
        }

        public bool Update(Listting obj)
        {
            return _ListtingDal.Update(obj);
        }

        public bool Delete(long id)
        {
            return _ListtingDal.Delete(id);
        }
    }
}
