using Core.Caching;
using Core.Helper.Cache;
using Core.Singleton;

namespace Core.Base
{
    public class BaseDal<T>
    {
        public T UnitOfWork;
        protected ICacheProvider Cache;
        protected CacheHelper CacheHelper;
        //protected string _schema;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDal{T}" /> class.
        /// </summary>
        /// <param name="schema">The schema.</param>
        public BaseDal()
        {
            //_schema = schema;
            //cache = new MemcachedProvider(schema);
            //cacheHelper = new CacheHelper(schema);
            UnitOfWork = (T)SingletonIpl.GetInstance<T>();
            //unitOfWork.CacheHelper = cacheHelper;
        }
    }
}
