using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class ProducerBll
    {
        private readonly ProducerDal _producerDal;

        public ProducerBll()
        {
            _producerDal = SingletonIpl.GetInstance<ProducerDal>();
        }


        public Producer GetProducer(long id)
        {
            return _producerDal.GetProducer(id);
        }

        public List<Producer> GetProducers()
        {
            return _producerDal.GetProducers();
        }

        public long Insert(Producer obj)
        {
            return _producerDal.Insert(obj);
        }

        public bool Update(Producer obj)
        {
            return _producerDal.Update(obj);
        }

        public bool Delete(long id)
        {
            return _producerDal.Delete(id);
        }
    }
}
