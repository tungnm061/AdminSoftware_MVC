using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;
using System.Transactions;
using System.Linq;
using Core.Helper.Logging;
using System;

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

        public bool Saves(List<GmailRemove> listObj)
        {
            bool result = true;
            List<GmailRemove> listCheck = _gmailRemoveDal.GetGmailRemoves(true);
            try
            {
                if (listObj == null || !listObj.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    foreach (GmailRemove item in listObj)
                    {
                        if (!listCheck.Any(x => x.GmailId == item.GmailId))
                        {
                            if (_gmailRemoveDal.Insert(item) <= 0)
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!_gmailRemoveDal.UpdateByGmailId(item))
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                    if (result)
                    {
                        scope.Complete();
                    }
                    else
                    {
                        scope.Dispose();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
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