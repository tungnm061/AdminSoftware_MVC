using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Sale;
using Entity.Sale;

namespace BusinessLogic.Sale
{
    public class ListtingBll
    {
        private readonly ListtingDal _listtingDal;
        public ListtingBll()
        {
            _listtingDal = SingletonIpl.GetInstance<ListtingDal>();
        }


        public Listting GetListting(long id)
        {
            return _listtingDal.GetListting(id);
        }


        public List<Listting> GetListtings(bool isActive = true, string keyWord = "")
        {
            return _listtingDal.GetListtings(isActive, keyWord);
        }

        public bool Saves(List<Listting> listObj)
        {
            bool result = true;
            List<Listting> listCheck = _listtingDal.GetListtings();
            try
            {
                if (listObj == null || !listObj.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    foreach (Listting item in listObj)
                    {
                        if (!listCheck.Any(x => x.GmailId == item.GmailId))
                        {
                            if (_listtingDal.Insert(item) <= 0)
                            {
                                result = false;
                                break;
                            }
                        }
                        else
                        {
                            if (!_listtingDal.UpdateByGmailId(item))
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

        public long Insert(Listting obj)
        {
            return _listtingDal.Insert(obj);
        }

        public bool Update(Listting obj)
        {
            return _listtingDal.Update(obj);
        }

        public bool Delete(long id)
        {
            return _listtingDal.Delete(id);
        }
    }
}
