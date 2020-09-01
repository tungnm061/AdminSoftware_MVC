using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Enum;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Helper;
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Controllers.Kpi
{
    public class TaskController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly CategoryKpiBll _categoryKpiBll;
        public TaskController()
        {
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
        }

        [SystemFilter]
        public ActionResult Index()
        {
            ViewBag.CategoryKpis =
                _categoryKpiBll.GetCategoryKpis()
                    .Select(x => new KendoForeignKeyModel(x.KpiName, x.CategoryKpiId.ToString()));
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            ViewBag.WorkPointConfigs =
                _workPointConfigBll.GetWorkPointConfigs()
                    .Select(x => new KendoForeignKeyModel(x.WorkPointName, x.WorkPointConfigId.ToString()));
            ViewBag.CalcType = from CalTypeNum s in Enum.GetValues(typeof (CalTypeNum))
                let singleOrDefault =
                    (DescriptionAttribute)
                        s.GetType()
                            .GetField(s.ToString())
                            .GetCustomAttributes(typeof (DescriptionAttribute), false)
                            .SingleOrDefault()
                where singleOrDefault != null
                select new KendoForeignKeyModel {value = ((byte) s).ToString(), text = singleOrDefault.Description};
            return View();
        }

        public JsonResult Tasks()
        {
            return Json(_taskBll.GetTasks(null, true, null, null), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Task(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new Task
                {
                    TaskId = Guid.Empty.ToString(),
                    CreateBy = UserLogin.UserId,
                    IsSystem = true,
                    CreateDate = DateTime.Now,
                    GroupName = "Công việc chung"
                });
            }
            return PartialView(_taskBll.GetTask(id));
        }

        public JsonResult Save(TaskModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var message = ModelState.Where(modelState => modelState.Value.Errors.Count > 0)
                        .Select(x => x.Value)
                        .FirstOrDefault();
                    return Json(new
                    {
                        Status = 0,
                        Message = message == null ? MessageAction.ModelStateNotValid : message.Errors[0].ErrorMessage
                    }, JsonRequestBehavior.AllowGet);
                }
                var task = _taskBll.GetTaskByTaskCode(model.TaskCode, model.TaskId);
                if (task != null)
                {
                    return Json(new {Status = 0, Message = "Mã công việc đã tồn tại trong hệ thống!"},
                        JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.TaskId) || model.TaskId == Guid.Empty.ToString())
                {
                    model.CreateDate = DateTime.Now;
                    model.TaskId = Guid.NewGuid().ToString();

                    return Json(_taskBll.Insert(model.ToObject())
                        ? new {Status = 1, Message = MessageAction.MessageCreateSuccess}
                        : new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(_taskBll.Update(model.ToObject())
                    ? new {Status = 1, Message = MessageAction.MessageUpdateSuccess}
                    : new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ImportExcel()
        {
            if (Request.Files["files"] == null)
                return Json(new {Status = 0, Message = "Không có dữ liệu để import!"}, JsonRequestBehavior.AllowGet);
            var stringCategoryKpi = Request.Form["CategoryKpiId"];
                   
            try
            {
                var file = Request.Files["files"];
                var dataLst = ExcelHelper.ReadExcelDictionary(file.InputStream);
                var tasks = new List<Task>();
                foreach (var data in dataLst)
                {                            
                    if (
                        data.ContainsKey("Mã công việc") &&
                        data.ContainsKey("Tên công việc") &&
                        data.ContainsKey("Giờ hữu ích") &&
                        data.ContainsKey("Mô tả") &&
                        data.ContainsKey("Điểm công việc"))
                    {
                        var point = string.IsNullOrEmpty(data["Điểm công việc"])
                            ? 0
                            : decimal.Parse(data["Điểm công việc"]);

                        var taskModel = new TaskModel
                        {
                            TaskCode = data["Mã công việc"],
                            TaskName = data["Tên công việc"],
                            GroupName = data["Nhóm công việc"],
                            UsefulHours = Decimal.Parse(data["Giờ hữu ích"]),
                            IsSystem = true,
                            Description = data["Mô tả"],
                            CreateBy = UserLogin.UserId,
                            CreateDate = DateTime.Now,
                            TaskId = Guid.NewGuid().ToString(),
                            WorkPointConfigId = 0,
                            CalcType = 8,
                            Frequent = true
                        };
                        if (string.IsNullOrEmpty(stringCategoryKpi))
                        {
                            taskModel.CategoryKpiId = null;
                        }
                        else
                        {
                            taskModel.CategoryKpiId = Int32.Parse(Request.Form["CategoryKpiId"]);
                        }
                        var workPointConfigs = _workPointConfigBll.GetWorkPointConfigs();
                        var workPointConfig = workPointConfigs.Where(x=>x.WorkPointA == 0).FirstOrDefault();
                        if (point == 0)
                        {
                            if (workPointConfig != null)
                                taskModel.WorkPointConfigId = workPointConfig.WorkPointConfigId;
                        }
                        else
                        {
                            foreach (var item in workPointConfigs)
                            {
                                if (point == item.WorkPointA)
                                {
                                    taskModel.WorkPointConfigId = item.WorkPointConfigId;
                                }
                            }
                        }

                        if (taskModel.WorkPointConfigId == 0)
                        {
                            return
                                Json(
                                    new
                                    {
                                        Status = 0,
                                        Message = "Trong danh sách có điểm công việc không tồn tại trong hệ thống!"
                                    },
                                    JsonRequestBehavior.AllowGet);
                        }

                        if (!TryValidateModel(taskModel))
                            return Json(new {Status = 0, Message = "Dữ liệu không hợp lệ!"},
                                JsonRequestBehavior.AllowGet);
                        var taskCheck = _taskBll.GetTaskByTaskCode(taskModel.TaskCode, taskModel.TaskId);
                        if (taskCheck != null)
                        {
                            return Json(new {Status = 0, Message = "Mã công việc đã tồn tại trong hệ thống!"},
                                JsonRequestBehavior.AllowGet);
                        }

                        tasks.Add(taskModel.ToObject());
                        //SaveLog((byte)ActionTypeEnum.Create, "Import hệ thống tà khoản", "ChartOfAccount");
                    }
                    else
                    {
                        return Json(new {Status = 0, Message = "File import không đúng định dạng!"},
                            JsonRequestBehavior.AllowGet);
                    }
                }
                if (_taskBll.Inserts(tasks))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageCreateSuccess},
                        JsonRequestBehavior.AllowGet);
                }

                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                if (_taskBll.Delete(id))
                {
                    return Json(new {Status = 1, Message = MessageAction.MessageDeleteSuccess},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new {Status = 0, Message = MessageAction.MessageActionFailed},
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new {Status = 0, ex.Message},
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TaskExcel()
        {
            return PartialView();
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}