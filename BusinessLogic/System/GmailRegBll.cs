using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;

namespace BusinessLogic.System
{
    public class GmailRegBll
    {
        private readonly GmailRegDal _gmailRegDal;

        public GmailRegBll()
        {
            _gmailRegDal = SingletonIpl.GetInstance<GmailRegDal>();
        }

        public List<GmailReg> GetGmailRegs(bool? isActive)
        {
            return _gmailRegDal.GetGmailRegs(isActive);
        }

        public GmailReg GetGmailReg(int id)
        {
            return _gmailRegDal.GetGmailReg(id);
        }

        public int Insert(GmailReg obj)
        {
            return _gmailRegDal.Insert(obj);
        }

        public int Update(GmailReg obj)
        {
            return _gmailRegDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _gmailRegDal.Delete(id);
        }
    }
}