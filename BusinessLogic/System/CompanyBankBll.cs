using System;
using DataAccess.System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Singleton;
using Entity.Common;
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

        public List<CompanyBank> GetCompanyBanks(bool? isActive, DateTime? fromDate, DateTime? toDate, int? expenseId, int? statusSearch)
        {
            return _companyBankDal.GetCompanyBanks(isActive, fromDate, toDate, expenseId, statusSearch);
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

        public bool UpdateConfirmDetail(CompanyBank obj)
        {
            return _companyBankDal.UpdateConfirmDetail(obj);
        }

        public BizResult UpdateConfirm(List<CompanyBank> listObj)
        {
            BizResult rs = new BizResult();
            rs.Status = 1;
            using (var scope = new TransactionScope())
            {
                foreach (var item in listObj)
                {
                    if (!_companyBankDal.UpdateConfirm(item))
                    {
                        rs.Status = -1;
                        break;
                    }
                }

                if (rs.Status == 1)
                {
                    scope.Complete();
                }
                else
                {
                    scope.Dispose();
                }

                return rs;
            }
        }
        public bool Delete(int id)
        {
            return _companyBankDal.Delete(id);
        }
    }
}
