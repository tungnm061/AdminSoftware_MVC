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
    public class JobChangeDal : BaseDal<ADOProvider>
    {
        public List<JobChange> GetJobChanges()
        {
            try
            {
                return UnitOfWork.Procedure<JobChange>("[hrm].[Get_JobChanges]").ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<JobChange>();
            }
        }

        public List<JobChange> GetJobChangesByEmployeeId(long employeeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", employeeId);
                return UnitOfWork.Procedure<JobChange>("[hrm].[Get_JobChange_ByEmployeeId]", param).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<JobChange>();
            }
        }

        public JobChange GetJobChange(string jobChangeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@JobChangeId", jobChangeId);
                return UnitOfWork.Procedure<JobChange>("[hrm].[Get_JobChange]", param).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(JobChange jobChange, ref string jobChangeCode)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CreateBy", jobChange.CreateBy);
                param.Add("@CreateDate", jobChange.CreateDate);
                param.Add("@Description", jobChange.Description);
                param.Add("@EmployeeId", jobChange.EmployeeId);
                param.Add("@FromDepartmentId", jobChange.FromDepartmentId);
                param.Add("@FromPositionId", jobChange.FromPositionId);
                param.Add("@JobChangeCode", jobChange.JobChangeCode, DbType.String, ParameterDirection.InputOutput);
                param.Add("@JobChangeFile", jobChange.JobChangeFile);
                param.Add("@JobChangeId", jobChange.JobChangeId);
                param.Add("@JobChangeNumber", jobChange.JobChangeNumber);
                param.Add("@Reason", jobChange.Reason);
                param.Add("@ToDepartmentId", jobChange.ToDepartmentId);
                param.Add("@ToPositionId", jobChange.ToPositionId);
                var result = UnitOfWork.ProcedureExecute("[hrm].[Insert_JobChange]", param);
                if (result)
                {
                    jobChangeCode = param.Get<string>("@JobChangeCode");
                }
                return result;
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(JobChange jobChange)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Description", jobChange.Description);
                param.Add("@EmployeeId", jobChange.EmployeeId);
                param.Add("@FromDepartmentId", jobChange.FromDepartmentId);
                param.Add("@FromPositionId", jobChange.FromPositionId);
                param.Add("@JobChangeFile", jobChange.JobChangeFile);
                param.Add("@JobChangeId", jobChange.JobChangeId);
                param.Add("@JobChangeNumber", jobChange.JobChangeNumber);
                param.Add("@Reason", jobChange.Reason);
                param.Add("@ToDepartmentId", jobChange.ToDepartmentId);
                param.Add("@ToPositionId", jobChange.ToPositionId);
                return UnitOfWork.ProcedureExecute("[hrm].[Update_JobChange]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string jobChangeId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@JobChangeId", jobChangeId);
                return UnitOfWork.ProcedureExecute("[hrm].[Delete_JobChange]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }
    }
}