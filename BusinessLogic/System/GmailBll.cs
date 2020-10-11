﻿using DataAccess.Sale;
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

        public List<Gmail> GetGmails(bool? isActive = true)
        {
            return _gmailDal.GetGmails(isActive);
        }

        public Gmail GetGmail(int GmailId)
        {
            return _gmailDal.GetGmail(GmailId);
        }

        public Gmail GetGmailByName(string gmailName)
        {
            return _gmailDal.GetGmailByName(gmailName);
        }

        public int Insert(Gmail obj)
        {
            return _gmailDal.Insert(obj);
        }

        public bool Inserts(List<Gmail> listObj)
        {
            return _gmailDal.Inserts(listObj);
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
