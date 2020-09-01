using System.Collections.Generic;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class CustomerBll
    {
        private readonly CustomerDal _customerDal;

        public CustomerBll()
        {
            _customerDal = SingletonIpl.GetInstance<CustomerDal>();
        }

        public Customer GetCustomer(long customerId)
        {
            return _customerDal.GetCustomer(customerId);
        }

        public List<Customer> GetCustomers(string keyword, int? status)
        {
            return _customerDal.GetCustomers(keyword, status);
        }

        public long Insert(Customer customer)
        {
            return _customerDal.Insert(customer);
        }

        public bool Update(Customer customer)
        {
            return _customerDal.Update(customer);
        }

        public bool Delete(long customerId)
        {
            return _customerDal.Delete(customerId);
        }
    }
}