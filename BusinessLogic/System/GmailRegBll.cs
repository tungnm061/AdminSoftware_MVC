using DataAccess.System;
using System.Collections.Generic;
using Core.Singleton;
using Entity.System;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using System;

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

        public bool Saves(List<GmailReg> listObj)
        {
            bool result = true;
            List<GmailReg> listCheck = _gmailRegDal.GetGmailRegs(true);
            try
            {
                if (listObj == null || !listObj.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    foreach (GmailReg item in listObj)
                    {
                        if (!listCheck.Any(x => x.GmailId == item.GmailId))
                        {
                            if (_gmailRegDal.Insert(item) <= 0)
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!_gmailRegDal.UpdateByGmailId(item))
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