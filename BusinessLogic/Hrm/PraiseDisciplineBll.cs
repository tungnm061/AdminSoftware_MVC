using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Hrm;
using Entity.Hrm;

namespace BusinessLogic.Hrm
{
    public class PraiseDisciplineBll
    {
        private readonly PraiseDisciplineDal _praiseDisciplineDal;

        public PraiseDisciplineBll()
        {
            _praiseDisciplineDal = SingletonIpl.GetInstance<PraiseDisciplineDal>();
        }

        public List<PraiseDiscipline> GetPraiseDisciplines(byte praiseDisciplineType)
        {
            return _praiseDisciplineDal.GetPraiseDisciplines(praiseDisciplineType);
        }

        public List<PraiseDiscipline> GetPraiseDisciplinesByEmployeeId(byte praiseDisciplineType, long employeeId)
        {
            return _praiseDisciplineDal.GetPraiseDisciplinesByEmployeeId(praiseDisciplineType, employeeId);
        }
        public PraiseDiscipline GetPraiseDiscipline(string praiseDisciplineId)
        {
            return _praiseDisciplineDal.GetPraiseDiscipline(praiseDisciplineId);
        }

        public bool Insert(PraiseDiscipline praiseDiscipline, ref string praiseDisciplineCode)
        {
            try
            {
                if (praiseDiscipline == null || !praiseDiscipline.PraiseDisciplineDetails.Any())
                    return false;
                using (var scope = new TransactionScope())
                {
                    if (_praiseDisciplineDal.Insert(praiseDiscipline, ref praiseDisciplineCode))
                    {
                        if (_praiseDisciplineDal.InsertDetail(praiseDiscipline.PraiseDisciplineId,
                            praiseDiscipline.PraiseDisciplineDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(PraiseDiscipline praiseDiscipline)
        {
            try
            {
                if (praiseDiscipline == null || !praiseDiscipline.PraiseDisciplineDetails.Any())
                    return false;
                using (var scope = new TransactionScope())
                {
                    if (_praiseDisciplineDal.Update(praiseDiscipline))
                    {
                        if (_praiseDisciplineDal.InsertDetail(praiseDiscipline.PraiseDisciplineId,
                            praiseDiscipline.PraiseDisciplineDetails))
                        {
                            scope.Complete();
                            return true;
                        }
                        scope.Dispose();
                        return false;
                    }
                    scope.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string praiseDisciplineId)
        {
            return _praiseDisciplineDal.Delete(praiseDisciplineId);
        }
    }
}