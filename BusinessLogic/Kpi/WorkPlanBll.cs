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
    public class WorkPlanBll
    {
        private readonly WorkPlanDal _workPlanDal;
        private readonly WorkPlanDetailDal _workPlanDetailDal;

        public WorkPlanBll()
        {
            _workPlanDal = SingletonIpl.GetInstance<WorkPlanDal>();
            _workPlanDetailDal = SingletonIpl.GetInstance<WorkPlanDetailDal>();
        }

        public List<WorkPlan> GetWorkPlansByDepartmentId(int action, string path, DateTime? formSearch,
            DateTime? toSearch)
        {
            return _workPlanDal.GetWorkPlansByDepartmentId(action, path, formSearch, toSearch);
        }
        public WorkPlan GetWorkPlanByToDate(DateTime toDate, int userId)
        {
            return _workPlanDal.GetWorkPlanByToDate(toDate, userId);
        }
        public List<WorkPlan> GetWorkPlansByIsActive()
        {
            return _workPlanDal.GetWorkPlansByIsActive();
        }
        public List<WorkPlan> GetWorkPlansByUserId(int userId, DateTime formSearch, DateTime toSearch)
        {
            return _workPlanDal.GetWorkPlansByUserId(userId, formSearch, toSearch);
        }

        public List<WorkPlan> GetWorkPlansNeedCompleted(decimal percentTime)
        {
            return _workPlanDal.GetWorkPlansNeedCompleted(percentTime);
        }

        public WorkPlan GetWorkPlanCheckDate(DateTime checkDate, string workPlanId, int userId)
        {
            return _workPlanDal.GetWorkPlanCheckDate(checkDate, workPlanId, userId);
        }

        public WorkPlan GetWorkPlanByDate(DateTime date, int userId)
        {
            return _workPlanDal.GetWorkPlanByDate(date, userId);
        }

        public WorkPlan GetWorkPlan(string workPlanId)
        {
            return _workPlanDal.GetWorkPlan(workPlanId);
        }

        public bool Delete(string workPlanId)
        {
            return _workPlanDal.Delete(workPlanId);
        }

        public bool Insert(WorkPlan workPlan, ref string workPlanCode)
        {
            try
            {
                if (workPlan == null || !workPlan.WorkPlanDetails.Any())
                {
                    return false;
                }

                using (var scope = new TransactionScope())
                {
                    if (_workPlanDal.Insert(workPlan, ref workPlanCode))
                    {
                        if (_workPlanDetailDal.Insert(workPlan.WorkPlanId, workPlan.WorkPlanDetails))
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

        public bool Update(WorkPlan workPlan)
        {
            try
            {
                if (workPlan == null || !workPlan.WorkPlanDetails.Any())
                {
                    return false;
                }
                var workPlanOld = _workPlanDal.GetWorkPlan(workPlan.WorkPlanId);
                if (workPlanOld == null)
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (_workPlanDal.Update(workPlan))
                    {
                        if (!_workPlanDetailDal.Deletes(workPlan.WorkPlanId))
                        {
                            scope.Dispose();
                            return false;
                        }
                        if (!_workPlanDetailDal.Insert(workPlan.WorkPlanId, workPlan.WorkPlanDetails))
                        {
                            scope.Dispose();
                            return false;
                        }
                    }
                    scope.Complete();
                    return true;
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