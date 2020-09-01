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
    public class TaskBll
    {
        private readonly TaskDal _taskDal;

        public TaskBll()
        {
            _taskDal = SingletonIpl.GetInstance<TaskDal>();
        }

        public List<Task> GetTasksByMissionId(int missionId)
        {
            return _taskDal.GetTasksByMissionId(missionId);
        }
        public Task GetTaskByTaskCode(string taskCode, string taskId)
        {
            return _taskDal.GetTaskByTaskCode(taskCode, taskId);
        }

        public List<Task> GetTasks(string keyword, bool? isSystem, long? categoryKpiId, int? userId)
        {
            return _taskDal.GetTasks(keyword, isSystem,categoryKpiId, userId);
        }

        public Task GetTask(string taskId)
        {
            return _taskDal.GetTask(taskId);
        }

        public bool Insert(Task task)
        {
            return _taskDal.Insert(task);
        }
        public bool Inserts(List<Task> tasks)
        {
            try
            {
                if (tasks == null || !tasks.Any())
                {
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    if (tasks.Select(task => _taskDal.Insert(task)).Any(insert => !insert))
                    {
                        scope.Dispose();
                        return false;
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
        public bool Update(Task task)
        {
            return _taskDal.Update(task);
        }

        public bool Delete(string taskId)
        {
            return _taskDal.Delete(taskId);
        }
    }
}