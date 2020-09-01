using System;
using System.Collections.Generic;
using System.Linq;
using Core.Base;
using Core.Data;
using Core.Helper.Logging;
using Dapper;
using Entity.Kpi;

namespace DataAccess.Kpi
{
    public class SuggesWorkDal : BaseDal<ADOProvider>
    {
        public List<SuggesWork> GetSuggesWorks(int? createBy, int action, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                return UnitOfWork.Procedure<SuggesWork>("[kpi].[Get_SuggesWorks]", new
                {
                    CreateBy = createBy,
                    Action = action,
                    FromDate = fromDate,
                    ToDate = toDate
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<SuggesWork>();
            }
        }


        public SuggesWork GetSuggesWork(string suggesWorkId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<SuggesWork>("[kpi].[Get_SuggesWork]", new {SuggesWorkId = suggesWorkId})
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public SuggesWork GetSuggesWorkByTaskId(string taskId, string suggesWorkId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<SuggesWork>("[kpi].[Get_SuggesWorkByTaskId]", new
                    {
                        TaskId = taskId,
                        SuggesWorkId = suggesWorkId
                    })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(SuggesWork suggesWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SuggesWorkId", suggesWork.SuggesWorkId);
                param.Add("@TaskId", suggesWork.TaskId);
                param.Add("@ToDate", suggesWork.ToDate);
                param.Add("@FromDate", suggesWork.FromDate);
                param.Add("@Status", suggesWork.Status);
                param.Add("@CreateBy", suggesWork.CreateBy);
                param.Add("@CreateDate", suggesWork.CreateDate);
                param.Add("@Description", suggesWork.Description);
                param.Add("@ApprovedFisnishDate", suggesWork.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", suggesWork.ApprovedFisnishBy);
                param.Add("@FisnishDate", suggesWork.FisnishDate);
                param.Add("@UsefulHours", suggesWork.UsefulHours);
                param.Add("@Explanation", suggesWork.Explanation);
                param.Add("@WorkingNote", suggesWork.WorkingNote);
                param.Add("@VerifiedBy", suggesWork.VerifiedBy);
                param.Add("@VerifiedDate", suggesWork.VerifiedDate);
                param.Add("@WorkPointType", suggesWork.WorkPointType);
                param.Add("@DepartmentFisnishBy", suggesWork.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", suggesWork.DepartmentFisnishDate);
                param.Add("@Quantity", suggesWork.Quantity);
                return (UnitOfWork.ProcedureExecute("[kpi].[Insert_SuggesWork]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(SuggesWork suggesWork)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@SuggesWorkId", suggesWork.SuggesWorkId);
                param.Add("@TaskId", suggesWork.TaskId);
                param.Add("@ToDate", suggesWork.ToDate);
                param.Add("@FromDate", suggesWork.FromDate);
                param.Add("@Status", suggesWork.Status);
                param.Add("@CreateBy", suggesWork.CreateBy);
                param.Add("@CreateDate", suggesWork.CreateDate);
                param.Add("@Description", suggesWork.Description);
                param.Add("@ApprovedFisnishDate", suggesWork.ApprovedFisnishDate);
                param.Add("@ApprovedFisnishBy", suggesWork.ApprovedFisnishBy);
                param.Add("@FisnishDate", suggesWork.FisnishDate);
                param.Add("@UsefulHours", suggesWork.UsefulHours);
                param.Add("@Explanation", suggesWork.Explanation);
                param.Add("@WorkingNote", suggesWork.WorkingNote);
                param.Add("@WorkPointType", suggesWork.WorkPointType);
                param.Add("@WorkPoint", suggesWork.WorkPoint);
                param.Add("@DepartmentFisnishBy", suggesWork.DepartmentFisnishBy);
                param.Add("@DepartmentFisnishDate", suggesWork.DepartmentFisnishDate);
                param.Add("@Quantity", suggesWork.Quantity);
                param.Add("@FileConfirm", suggesWork.FileConfirm);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_SuggesWork]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string suggesWorkId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_SuggesWork]", new
                {
                    SuggesWorkId = suggesWorkId
                });
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}