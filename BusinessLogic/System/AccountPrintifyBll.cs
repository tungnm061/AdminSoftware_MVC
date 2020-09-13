using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;
namespace BusinessLogic.System
{
    public class AccountPrintifyBll
    {
        private readonly AccountPrintifyDal _accountPrintifyDal;

        public AccountPrintifyBll()
        {
            _accountPrintifyDal = SingletonIpl.GetInstance<AccountPrintifyDal>();
        }

        public List<AccountPrintify> GetAccountPrintifys()
        {
            return _accountPrintifyDal.GetAccountPrintifys();
        }

        public AccountPrintify GetAccountPrintify(int AccountPrintifyId)
        {
            return _accountPrintifyDal.GetAccountPrintify(AccountPrintifyId);
        }

        public int Insert(AccountPrintify obj)
        {
            return _accountPrintifyDal.Insert(obj);
        }

        public bool Update(AccountPrintify obj)
        {
            return _accountPrintifyDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _accountPrintifyDal.Delete(id);
        }
    }
}
