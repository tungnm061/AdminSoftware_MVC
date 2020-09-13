using DataAccess.Sale;
using System.Collections.Generic;
using Core.Singleton;
using DataAccess.System;
using Entity.Sale;
using Entity.System;

namespace BusinessLogic.System
{
    public class GmailBll
    {
        private readonly GmailDal _gmailDal;

        public GmailBll()
        {
            _gmailDal = SingletonIpl.GetInstance<GmailDal>();
        }

        public List<Gmail> GetGmails()
        {
            return _gmailDal.GetGmails();
        }

        public Gmail GetGmail(int GmailId)
        {
            return _gmailDal.GetGmail(GmailId);
        }

        public int Insert(Gmail obj)
        {
            return _gmailDal.Insert(obj);
        }

        public bool Update(Gmail obj)
        {
            return _gmailDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _gmailDal.Delete(id);
        }

    }
}
