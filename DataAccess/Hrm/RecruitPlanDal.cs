using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Hrm;

namespace DataAccess.Hrm
{
    public class RecruitPlanDal : BaseDal<ADOProvider>
    {
        public List<RecruitPlan> GetRecruitPlans(bool? isActive, DateTime fromDate, DateTime toDate)
        {
            try
            {
                return UnitOfWork.Procedure<RecruitPlan>("[hrm].[Get_RecruitPlans]", new
                {
                    IsActive = isActive,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<RecruitPlan>();
            }
        }

        public RecruitPlan GetRecruitPlan(long recruitPlanId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<RecruitPlan>("[hrm].[Get_RecruitPlan]", new {RecruitPlanId = recruitPlanId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public long Insert(RecruitPlan recruitPlan, ref string recruitPlanCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ChanelIds", recruitPlan.ChanelIds);
                param.Add("@CreateBy", recruitPlan.CreateBy);
                param.Add("@CreateDate", recruitPlan.CreateDate);
                param.Add("@DepartmentId", recruitPlan.DepartmentId);
                param.Add("@Description", recruitPlan.Description);
                param.Add("@FromDate", recruitPlan.FromDate);
                param.Add("@IsActive", recruitPlan.IsActive);
                param.Add("@PositionId", recruitPlan.PositionId);
                param.Add("@Quantity", recruitPlan.Quantity);
                param.Add("@RecruitPlanCode", recruitPlan.RecruitPlanCode, DbType.String, ParameterDirection.InputOutput);
                param.Add("@RecruitPlanId", recruitPlan.RecruitPlanId, DbType.Int64, ParameterDirection.Output);
                param.Add("@Requirements", recruitPlan.Requirements);
                param.Add("@Title", recruitPlan.Title);
                param.Add("@ToDate", recruitPlan.ToDate);
                if (UnitOfWork.ProcedureExecute("[hrm].[Insert_RecruitPlan]", param))
                {
                    recruitPlanCode = param.Get<string>("@RecruitPlanCode");
                    return param.Get<long>("@RecruitPlanId");
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return 0;
            }
        }

        public bool Update(RecruitPlan recruitPlan)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@ChanelIds", recruitPlan.ChanelIds);
                param.Add("@CreateBy", recruitPlan.CreateBy);
                param.Add("@CreateDate", recruitPlan.CreateDate);
                param.Add("@DepartmentId", recruitPlan.DepartmentId);
                param.Add("@Description", recruitPlan.Description);
                param.Add("@FromDate", recruitPlan.FromDate);
                param.Add("@IsActive", recruitPlan.IsActive);
                param.Add("@PositionId", recruitPlan.PositionId);
                param.Add("@Quantity", recruitPlan.Quantity);
                param.Add("@RecruitPlanCode", recruitPlan.RecruitPlanCode);
                param.Add("@RecruitPlanId", recruitPlan.RecruitPlanId);
                param.Add("@Requirements", recruitPlan.Requirements);
                param.Add("@Title", recruitPlan.Title);
                param.Add("@ToDate", recruitPlan.ToDate);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_RecruitPlan]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(long recruitPlanId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RecruitPlanId", recruitPlanId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_RecruitPlan]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}