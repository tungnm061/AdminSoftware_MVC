using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class WorkStreamBll
    {
        private readonly PerformerDal _performerDal;
        private readonly WorkStreamDal _workStreamDal;
        private readonly WorkStreamDetailDal _workStreamDetailDal;

        public WorkStreamBll()
        {
            _workStreamDal = SingletonIpl.GetInstance<WorkStreamDal>();
            _workStreamDetailDal = SingletonIpl.GetInstance<WorkStreamDetailDal>();
            _performerDal = SingletonIpl.GetInstance<PerformerDal>();
        }

        public List<WorkStream> GetWorkStreamsByUserId(int? userId, DateTime? formSearch, DateTime? toSearch, int action)
        {
            return _workStreamDal.GetWorkStreamsByUserId(userId, formSearch, toSearch, action);
        }

        public List<WorkStream> GetWorkStreamsNeedVerify()
        {
            return _workStreamDal.GetWorkStreamsNeedVerify();
        }

        public WorkStream GetWorkStreamCheckDate(DateTime checkDate, string workStream)
        {
            return _workStreamDal.GetWorkStreamCheckDate(checkDate, workStream);
        }

        public WorkStream GetWorkStream(string workStream)
        {
            return _workStreamDal.GetWorkStream(workStream);
        }

        public bool Delete(string workStream)
        {
            return _workStreamDal.Delete(workStream);
        }

        public bool Insert(WorkStream workStream, ref string workStreamCode)
        {
            try
            {
                if (workStream == null || !workStream.WorkStreamDetails.Any())
                {
                    return false;
                }

                using (var scope = new TransactionScope())
                {
                    if (_workStreamDal.Insert(workStream, ref workStreamCode))
                    {
                        if (!_workStreamDetailDal.Inserts(workStream.WorkStreamId, workStream.WorkStreamDetails))
                        {
                            scope.Dispose();
                            return false;
                        }
                        if (!_performerDal.Inserts(workStream.WorkStreamId, workStream.Performers))
                        {
                            scope.Dispose();
                            return false;
                        }
                        scope.Complete();
                        return true;
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

        public bool Update(WorkStream workStream)
        {
            try
            {
                if (workStream == null)
                {
                    return false;
                }
                var workPlanOld = _workStreamDal.GetWorkStream(workStream.WorkStreamId);
                if (workPlanOld == null)
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_workStreamDal.Update(workStream))
                    {
                        if (workStream.WorkStreamDetails.Any())
                        {
                            if (!_workStreamDetailDal.Inserts(workStream.WorkStreamId, workStream.WorkStreamDetails))
                            {
                                scope.Dispose();
                                return false;
                            }
                        }

                        if (!_performerDal.Inserts(workStream.WorkStreamId, workStream.Performers))
                        {
                            scope.Dispose();
                            return false;
                        }
                        scope.Complete();
                        return true;
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
    }
}