using System;
using System.Collections.Generic;
using System.Transactions;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using DataAccess.Kpi;
using Entity.Kpi;

namespace BusinessLogic.Kpi
{
    public class SuggesWorkBll
    {
        private readonly SuggesWorkDal _suggesWorkDal;
        private readonly AssignWorkDal _assignWorkDal;
        private readonly WorkPlanDetailDal _workPlanDetailDal;
        private readonly WorkStreamDetailDal _workStreamDetailDal;

        public SuggesWorkBll()
        {
            _suggesWorkDal = SingletonIpl.GetInstance<SuggesWorkDal>();
            _workStreamDetailDal = SingletonIpl.GetInstance<WorkStreamDetailDal>();
            _assignWorkDal = SingletonIpl.GetInstance<AssignWorkDal>();
            _workPlanDetailDal = SingletonIpl.GetInstance<WorkPlanDetailDal>();
        }
        public SuggesWork GetSuggesWorkByTaskId(string taskId, string suggesWorkId)
        {
            return _suggesWorkDal.GetSuggesWorkByTaskId(taskId, suggesWorkId);
        }

        public List<SuggesWork> GetSuggesWorks(int? createBy, int action, DateTime? fromDate, DateTime? toDate)
        {
            return _suggesWorkDal.GetSuggesWorks(createBy, action, fromDate, toDate);
        }

        public SuggesWork GetSuggesWork(string suggesWorkId)
        {
            return _suggesWorkDal.GetSuggesWork(suggesWorkId);
        }

        public bool Insert(SuggesWork suggesWork)
        {
            return _suggesWorkDal.Insert(suggesWork);
        }
        public bool InsertTransection(SuggesWork suggesWork,WorkDetail workDetail)
        {
            try
            {
                if (suggesWork == null )
                {
                    return false;
                }
                if (workDetail == null )
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_suggesWorkDal.Insert(suggesWork))
                    {
                        if (workDetail.WorkType == (int) StatusWorkDetail.WorkPlanDetail)
                        {
                            var workPlanDetail = _workPlanDetailDal.GetWorkPlanDetail(workDetail.WorkDetailId);
                            workPlanDetail.Status = 5;
                            workPlanDetail.DepartmentFisnishBy = workPlanDetail.CreateBy;
                            workPlanDetail.DepartmentFisnishDate = DateTime.Now;
                            if (_workPlanDetailDal.Update(workPlanDetail))
                            {
                                scope.Complete();
                                return true;
                            }
                            scope.Dispose();
                            return false;
                        }
                        if (workDetail.WorkType == (int)StatusWorkDetail.AssignWork)
                        {
                            var assignWork = _assignWorkDal.GetAssignWork(workDetail.WorkDetailId);
                            assignWork.Status = 5;
                            assignWork.DepartmentFisnishBy = assignWork.CreateBy;
                            assignWork.DepartmentFisnishDate = DateTime.Now;
                            if (_assignWorkDal.Update(assignWork))
                            {
                                scope.Complete();
                                return true;
                            }
                            scope.Dispose();
                            return false;
                        }
                        if (workDetail.WorkType == (int)StatusWorkDetail.SuggesWork)
                        {
                            var suggesWorkUpdate = _suggesWorkDal.GetSuggesWork(workDetail.WorkDetailId);
                            suggesWorkUpdate.Status = 5;
                            suggesWorkUpdate.DepartmentFisnishBy = suggesWorkUpdate.CreateBy;
                            suggesWorkUpdate.DepartmentFisnishDate = DateTime.Now;
                            if (_suggesWorkDal.Update(suggesWorkUpdate))
                            {
                                scope.Complete();
                                return true;
                            }
                            scope.Dispose();
                            return false;
                        }
                        if (workDetail.WorkType == (int)StatusWorkDetail.WorkStreamDetail)
                        {
                            var workStreamDetail = _workStreamDetailDal.GetWorkStreamDetail(workDetail.WorkDetailId);
                            workStreamDetail.Status = 5;
                            if (_workStreamDetailDal.Update(workStreamDetail))
                            {
                                scope.Complete();
                                return true;
                            }
                            scope.Dispose();
                            return false;
                        }
                        scope.Complete();
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

        public bool Update(SuggesWork suggesWork)
        {
            return _suggesWorkDal.Update(suggesWork);
        }

        public bool Delete(string suggesWorkId)
        {
            return _suggesWorkDal.Delete(suggesWorkId);
        }
    }
}