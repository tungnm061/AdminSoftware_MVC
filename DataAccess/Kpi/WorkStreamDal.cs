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
    public class WorkStreamDal : BaseDal<ADOProvider>
    {
        public List<WorkStream> GetWorkStreamsByUserId(int? userId, DateTime? formSearch, DateTime? toSearch, int action)
        {
            return UnitOfWork.Procedure<WorkStream>("[kpi].[Get_WorkStreams]", new
            {
                UserId = userId,
                FromDate = formSearch,
                ToDate = toSearch,
                Action = action
            }).ToList();
        }
        public List<WorkStream> GetWorkStreamsNeedVerify()
        {
            return UnitOfWork.Procedure<WorkStream>("[kpi].[Get_WorkStreams_NeedVerify]").ToList();
        }

        public WorkStream GetWorkStreamCheckDate(DateTime checkDate, string workStreamId)
        {
            return UnitOfWork.Procedure<WorkStream>("[kpi].[Get_WorkStreamCheckDate]", new
            {
                CheckDate = checkDate,
                WorkStreamId = workStreamId
            }).FirstOrDefault();
        }

        public WorkStream GetWorkStream(string workStreamId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStreamId);
                var multi = UnitOfWork.ProcedureQueryMulti("[kpi].[Get_WorkStream]", param);
                var workStream = multi.Read<WorkStream>().FirstOrDefault();
                if (workStream != null)
                {
                    workStream.WorkStreamDetails = multi.Read<WorkStreamDetail>().ToList();
                    workStream.Performers = multi.Read<Performer>().ToList();
                }
                return workStream;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(WorkStream workStream, ref string workStreamCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStream.WorkStreamId);
                param.Add("@CreateBy", workStream.CreateBy);
                param.Add("@FromDate", workStream.FromDate);
                param.Add("@ToDate", workStream.ToDate);
                param.Add("@CreateDate", workStream.CreateDate);
                param.Add("@WorkStreamCode", workStream.WorkStreamCode, DbType.String,
                    ParameterDirection.InputOutput);
                param.Add("@Description", workStream.Description);
                param.Add("@AssignWorkId", workStream.AssignWorkId);
                param.Add("@Status", workStream.Status);
                param.Add("@TaskId", workStream.TaskId);
                param.Add("@ApprovedBy", workStream.ApprovedBy);
                param.Add("@ApprovedDate", workStream.ApprovedDate);
                var result = UnitOfWork.ProcedureExecute("[kpi].[Insert_WorkStream]", param);
                if (result)
                {
                    workStreamCode = param.Get<string>("@WorkStreamCode");
                }
                return result;
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
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStream.WorkStreamId);
                param.Add("@CreateBy", workStream.CreateBy);
                param.Add("@FromDate", workStream.FromDate);
                param.Add("@ToDate", workStream.ToDate);
                param.Add("@CreateDate", workStream.CreateDate);
                param.Add("@WorkStreamCode", workStream.WorkStreamCode, DbType.String,
                    ParameterDirection.InputOutput);
                param.Add("@Description", workStream.Description);
                param.Add("@AssignWorkId", workStream.AssignWorkId);
                param.Add("@Status", workStream.Status);
                param.Add("@TaskId", workStream.TaskId);
                param.Add("@ApprovedBy", workStream.ApprovedBy);
                param.Add("@ApprovedDate", workStream.ApprovedDate);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_WorkStream]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string workStreamId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@WorkStreamId", workStreamId);
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_WorkStream]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}