using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;

namespace BusinessLogic.System
{
    public class GmailRemoveBll
    {
        private readonly GmailRemoveDal _gmailRemoveDal;

        public GmailRemoveBll()
        {
            _gmailRemoveDal = SingletonIpl.GetInstance<GmailRemoveDal>();
        }

        public List<GmailRemove> GetGmailRemoves(bool? isActive)
        {
            return _gmailRemoveDal.GetGmailRemoves(isActive);
        }

        public GmailRemove GetGmailRemove(int id)
        {
            return _gmailRemoveDal.GetGmailRemove(id);
        }

        public int Insert(GmailRemove obj)
        {
            return _gmailRemoveDal.Insert(obj);
        }

        public int Update(GmailRemove obj)
        {
            return _gmailRemoveDal.Update(obj);
        }

        public bool Delete(int id)
        {
            return _gmailRemoveDal.Delete(id);
        }
    }
}