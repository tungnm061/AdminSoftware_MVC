using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class WorkPlanDetailDal : BaseDal<ADOProvider>
    {
        public List<WorkPlanDetail> GetWorkPlanDetailsNeedCompleted(DateTime date)
        {
            try
            {
                return UnitOfWork.Procedure<WorkPlanDetail>("[kpi].[Get_WorkPlanDetail_NeedCompleted]", new
                {
                    Date = date
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<WorkPlanDetail>();
            }
        }

        public WorkPlanDetail GetWorkPlanDetail(string workPlanDetailId)
        {
            return
                UnitOfWork.Procedure<WorkPlanDetail>("[kpi].[Get_WorkPlanDetail]",
                    new {WorkPlanDetailId = workPlanDetailId}).FirstOrDefault();
        }

        public bool Deletes(string workPlanId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlanId);
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_WorkPlanDetails]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(WorkPlanDetail workPlanDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanDetailId", workPlanDetail.WorkPlanDetailId);
                param.Add("@WorkPlanId", workPlanDetail.WorkPlanId);
                param.Add("@TaskId", workPlanDetail.TaskId);
                param.Add("@Description", workPlanDetail.Description);
                param.Add("@Explanation", workPlanDetail.Explanation);
                param.Add("@FromDate", workPlanDetail.FromDate);
                param.Add("@ToDate", workPlanDetail.ToDate);
                param.Add("@Status", workPlanDetail.Status);
                param.Add("@UsefulHours", workPlanDetail.UsefulHours);
                param.Add("@WorkingNote", workPlanDetail.WorkingNote);
                param.Add("@ApprovedFisnishBy", workPlanDetail.ApprovedFisnishBy);
                param.Add("@ApprovedFisnishDate", workPlanDetail.ApprovedFisnishDate);
                param.Add("@FisnishDate", workPlanDetail.FisnishDate);
                param.Add("@WorkPointType", workPlanDetail.WorkPointType);
                param.Add("@WorkPoint", workPlanDetail.WorkPoint);
                param.Add("@DepartmentFisnishBy", workPlanDetail.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", workPlanDetail.DepartmentFisnishDate);
                param.Add("@Quantity", workPlanDetail.Quantity);
                param.Add("@FileConfirm", workPlanDetail.FileConfirm);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_WorkPlanDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Insert(string workPlanId, List<WorkPlanDetail> workPlanDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkPlanId", workPlanId);
                param.Add("@XML", XmlHelper.SerializeXml<List<WorkPlanDetail>>(workPlanDetails));
                return UnitOfWork.ProcedureExecute("[kpi].[Insert_WorkPlanDetails]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}