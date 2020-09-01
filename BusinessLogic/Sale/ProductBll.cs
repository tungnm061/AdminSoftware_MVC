using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class ProductBll
    {
        private readonly ProductDal _productDal;

        public ProductBll()
        {
            _productDal = SingletonIpl.GetInstance<ProductDal>();
        }

        public List<Product> GetProductsSell()
        {
            return _productDal.GetProductsSell();
        }

        public Product GetProduct(long productId)
        {
            return _productDal.GetProduct(productId);
        }

        public List<Product> GetProducts(bool? isActive, string keyword)
        {
            return _productDal.GetProducts(isActive, keyword);
        }

        public long Insert(Product product)
        {
            return _productDal.Insert(product);
        }

        public bool Update(Product product)
        {
            return _productDal.Update(product);
        }

        public bool Delete(long productId)
        {
            return _productDal.Delete(productId);
        }
    }
}