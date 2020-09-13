using DataAccess.Sale;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.Sale;
using Entity.System;

namespace BusinessLogic.System
{
    public class SkuBll
    {
        private readonly SkuDal _skuDal;

        public SkuBll()
        {
            _skuDal = SingletonIpl.GetInstance<SkuDal>();
        }

        public List<Sku> GetSkus(int gmailId)
        {
            return _skuDal.GetSkus(gmailId);
        }

        public Sku GetSku(int SkuId)
        {
            return _skuDal.GetSku(SkuId);
        }

        public int Insert(Sku obj)
        {
            return _skuDal.Insert(obj);
        }

        public bool Update(Sku obj)
        {
            return _skuDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _skuDal.Delete(id);
        }

    }
}
