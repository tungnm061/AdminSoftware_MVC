using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class InvestorBll
    {
        private readonly InvestorDal _investorDal;

        public InvestorBll()
        {
            _investorDal = SingletonIpl.GetInstance<InvestorDal>();
        }

        public List<Investor> GetInvestors()
        {
            return _investorDal.GetInvestors();
        }

        public Investor GetInvestor(string investorId)
        {
            return _investorDal.GetInvestor(investorId);
        }

        public bool Insert(Investor investor)
        {
            return _investorDal.Insert(investor);
        }

        public bool Update(Investor investor)
        {
            return _investorDal.Update(investor);
        }

        public bool Delete(string investorId)
        {
            return _investorDal.Delete(investorId);
        }
    }
}
