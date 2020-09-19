using System;
using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;

namespace BusinessLogic.System
{
    public class CompanyBankBll
    {
        private readonly CompanyBankDal _companyBankDal;

        public CompanyBankBll()
        {
            _companyBankDal = SingletonIpl.GetInstance<CompanyBankDal>();
        }

        public List<CompanyBank> GetCompanyBanks(bool? isActive, DateTime? fromDate, DateTime? toDate, int? expenseId)
        {
            return _companyBankDal.GetCompanyBanks(isActive, fromDate, toDate, expenseId);
        }

        public CompanyBank GetCompanyBank(int CompanyBankId)
        {
            return _companyBankDal.GetCompanyBank(CompanyBankId);
        }

        public int Insert(CompanyBank obj)
        {
            return _companyBankDal.Insert(obj);
        }

        public bool Update(CompanyBank obj)
        {
            return _companyBankDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _companyBankDal.Delete(id);
        }
    }
}
