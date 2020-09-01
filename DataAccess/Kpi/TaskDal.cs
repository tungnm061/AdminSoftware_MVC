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
    public class TaskDal : BaseDal<ADOProvider>
    {
        public List<Task> GetTasks(string keyword, bool? isSystem, long? categoryKpiId, int? userId)
        {
            try
            {
                return UnitOfWork.Procedure<Task>("[kpi].[Get_Tasks]", new
                {
                    Keyword = keyword,
                    IsSystem = isSystem,
                    CategoryKpiId = categoryKpiId,
                    UserId = userId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Task>();
            }
        }
        public List<Task> GetTasksByMissionId(int missionId)
        {
            try
            {
                return UnitOfWork.Procedure<Task>("[kpi].[Get_Tasks_ByMissionId]", new
                {
                    MissionId = missionId
                }).ToList();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return new List<Task>();
            }
        }
        public Task GetTask(string taskId)
        {
            try
            {
                return UnitOfWork.Procedure<Task>("[kpi].[Get_Task]", new {TaskId = taskId}).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public Task GetTaskByTaskCode(string taskCode, string taskId)
        {
            try
            {
                return
                    UnitOfWork.Procedure<Task>("[kpi].[Get_Task_ByTaskCode]", new
                    {
                        TaskCode = taskCode,
                        TaskId = taskId
                    })
                        .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return null;
            }
        }

        public bool Insert(Task task)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TaskId", task.TaskId);
                param.Add("@TaskCode", task.TaskCode);
                param.Add("@TaskName", task.TaskName);
                param.Add("@CalcType", task.CalcType);
                param.Add("@WorkPointConfigId", task.WorkPointConfigId);
                param.Add("@UsefulHours", task.UsefulHours);
                param.Add("@Frequent", task.Frequent);
                param.Add("@Description", task.Description);
                param.Add("@IsSystem", task.IsSystem);
                param.Add("@CreateDate", task.CreateDate);
                param.Add("@CreateBy", task.CreateBy);
                param.Add("@GroupName", task.GroupName);
                param.Add("@CategoryKpiId", task.CategoryKpiId);
                return (UnitOfWork.ProcedureExecute("[kpi].[Insert_Task]", param));
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Update(Task task)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@TaskId", task.TaskId);
                param.Add("@TaskCode", task.TaskCode);
                param.Add("@TaskName", task.TaskName);
                param.Add("@CalcType", task.CalcType);
                param.Add("@WorkPointConfigId", task.WorkPointConfigId);
                param.Add("@UsefulHours", task.UsefulHours);
                param.Add("@Frequent", task.Frequent);
                param.Add("@Description", task.Description);
                param.Add("@IsSystem", task.IsSystem);
                param.Add("@CreateDate", task.CreateDate);
                param.Add("@CreateBy", task.CreateBy);
                param.Add("@GroupName", task.GroupName);
                param.Add("@CategoryKpiId", task.CategoryKpiId);
                return UnitOfWork.ProcedureExecute("[kpi].[Update_Task]", param);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return false;
            }
        }

        public bool Delete(string taskId)
        {
            try
            {
                return UnitOfWork.ProcedureExecute("[kpi].[Delete_Task]", new
                {
                    TaskId = taskId
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