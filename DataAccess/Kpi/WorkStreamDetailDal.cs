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
    public class WorkStreamDetailDal : BaseDal<ADOProvider>
    {
        public WorkStreamDetail GetWorkStreamDetail(string workStreamDetailId)
        {
            return
                UnitOfWork.Procedure<WorkStreamDetail>("[kpi].[Get_WorkStreamDetail]",
                    new {WorkStreamDetailId = workStreamDetailId}).FirstOrDefault();
        }
        public bool Delete(string workStreamDetailId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamDetailId", workStreamDetailId);
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_WorkStreamDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Insert(WorkStreamDetail workStreamDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamDetailId", workStreamDetail.WorkStreamDetailId);
                param.Add("@TaskId", workStreamDetail.TaskId);
                param.Add("@WorkStreamId", workStreamDetail.WorkStreamId);
                param.Add("@CreateBy", workStreamDetail.CreateBy);
                param.Add("@CreateDate", workStreamDetail.CreateDate);
                param.Add("@FromDate", workStreamDetail.FromDate);
                param.Add("@ToDate", workStreamDetail.ToDate);
                param.Add("@Status", workStreamDetail.Status);
                param.Add("@Description", workStreamDetail.Description);
                param.Add("@UsefulHours", workStreamDetail.UsefulHours);
                param.Add("@IsDefault", workStreamDetail.IsDefault);
                param.Add("@ApprovedFisnishDate", workStreamDetail.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", workStreamDetail.ApprovedFisnishBy);
                param.Add("@FisnishDate", workStreamDetail.FisnishDate);
                param.Add("@WorkingNote", workStreamDetail.WorkingNote);
                param.Add("@Explanation", workStreamDetail.Explanation);
                param.Add("@WorkPointType", workStreamDetail.WorkPointType);
                param.Add("@DepartmentFisnishBy", workStreamDetail.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", workStreamDetail.DepartmentFisnishDate);
                param.Add("@Quantity", workStreamDetail.Quantity);
                return UnitOfWork.ProcedureExecute("[kpi].[Insert_WorkStreamDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(WorkStreamDetail workStreamDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamDetailId", workStreamDetail.WorkStreamDetailId);
                param.Add("@TaskId", workStreamDetail.TaskId);
                param.Add("@WorkStreamId", workStreamDetail.WorkStreamId);
                param.Add("@CreateBy", workStreamDetail.CreateBy);
                param.Add("@CreateDate", workStreamDetail.CreateDate);
                param.Add("@FromDate", workStreamDetail.FromDate);
                param.Add("@ToDate", workStreamDetail.ToDate);
                param.Add("@Status", workStreamDetail.Status);
                param.Add("@Despcription", workStreamDetail.Description);
                param.Add("@UsefulHours", workStreamDetail.UsefulHours);
                param.Add("@IsDefault", workStreamDetail.IsDefault);
                param.Add("@ApprovedFisnishDate", workStreamDetail.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", workStreamDetail.ApprovedFisnishBy);
                param.Add("@FisnishDate", workStreamDetail.FisnishDate);
                param.Add("@WorkingNote", workStreamDetail.WorkingNote);
                param.Add("@Explanation", workStreamDetail.Explanation);
                param.Add("@WorkPointType", workStreamDetail.WorkPointType);
                param.Add("@WorkPoint", workStreamDetail.WorkPoint);
                param.Add("@DepartmentFisnishBy", workStreamDetail.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", workStreamDetail.DepartmentFisnishDate);
                param.Add("@Quantity", workStreamDetail.Quantity);
                param.Add("@FileConfirm", workStreamDetail.FileConfirm);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_WorkStreamDetail]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Inserts(string workStreamId, List<WorkStreamDetail> workStreamDetails)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStreamId);
                param.Add("@XML", XmlHelper.SerializeXml<List<WorkStreamDetail>>(workStreamDetails));
                return UnitOfWork.ProcedureExecute("[kpi].[Insert_WorkStreamDetails]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}