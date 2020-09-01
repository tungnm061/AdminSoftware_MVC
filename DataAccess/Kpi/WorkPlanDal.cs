using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class WorkPlanDal : BaseDal<ADOProvider>
    {
        public List<WorkPlan> GetWorkPlansByUserId(int userId, DateTime formSearch, DateTime toSearch)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlans]", new
            {
                UserId = userId,
                FromDate = formSearch,
                ToDate = toSearch
            }).ToList();
        }

        public List<WorkPlan> GetWorkPlansNeedCompleted(decimal percentTime)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlan_NeedComplete]", new
            {
                PercentTime = percentTime
            }).ToList();
        }

        public List<WorkPlan> GetWorkPlansByDepartmentId(int action, string path, DateTime? formSearch,
            DateTime? toSearch)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlans_ByDepartmentId]", new
            {
                Path = path,
                Action = action,
                FromDate = formSearch,
                ToDate = toSearch
            }).ToList();
        }

        public List<WorkPlan> GetWorkPlansByIsActive()
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlans_IsActive]").ToList();
        }

        public WorkPlan GetWorkPlanCheckDate(DateTime checkDate, string workPlanId,int userId)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlanCheckDate]", new
            {
                CheckDate = checkDate,
                WorkPlanId = workPlanId,
                UserId = userId
            }).FirstOrDefault();
        }
        public WorkPlan GetWorkPlanByToDate(DateTime toDate, int userId)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlans_ByToDate]", new
            {
                UserId = userId,
                ToDateOld = toDate
            }).FirstOrDefault();
        }
        public WorkPlan GetWorkPlanByDate(DateTime date, int userId)
        {
            return UnitOfWork.Procedure<WorkPlan>("[kpi].[Get_WorkPlan_ByDate]", new
            {
                Date = date,
                UserId = userId
            }).FirstOrDefault();
        }

        public WorkPlan GetWorkPlan(string workPlanId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlanId);
                var multi = UnitOfWork.ProcedureQueryMulti("[kpi].[Get_WorkPlan]", param);
                var workPlan = multi.Read<WorkPlan>().FirstOrDefault();
                if (workPlan != null)
                {
                    workPlan.WorkPlanDetails = multi.Read<WorkPlanDetail>().ToList();
                }
                return workPlan;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(WorkPlan workPlan, ref string workPlanCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlan.WorkPlanId);
                param.Add("@CreateBy", workPlan.CreateBy);
                param.Add("@FromDate", workPlan.FromDate);
                param.Add("@ToDate", workPlan.ToDate);
                param.Add("@CreateDate", workPlan.CreateDate);
                param.Add("@WorkPlanCode", workPlan.WorkPlanCode, DbType.String,
                    ParameterDirection.InputOutput);
                param.Add("@Description", workPlan.Description);
                param.Add("@ConfirmedBy", workPlan.ConfirmedBy);
                param.Add("@ApprovedBy", workPlan.ApprovedBy);
                param.Add("@ConfirmedDate", workPlan.ConfirmedDate);
                param.Add("@ApprovedDate", workPlan.ApprovedDate);
                var result = UnitOfWork.ProcedureExecute("[kpi].[Insert_WorkPlan]", param);
                if (result)
                {
                    workPlanCode = param.Get<string>("@WorkPlanCode");
                }
                return result;
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
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlan.WorkPlanId);
                param.Add("@CreateBy", workPlan.CreateBy);
                param.Add("@FromDate", workPlan.FromDate);
                param.Add("@ToDate", workPlan.ToDate);
                param.Add("@CreateDate", workPlan.CreateDate);
                param.Add("@WorkPlanCode", workPlan.WorkPlanCode, DbType.String,
                    ParameterDirection.InputOutput);
                param.Add("@Description", workPlan.Description);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_WorkPlan]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string workPlanId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlanId);
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_WorkPlan]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}