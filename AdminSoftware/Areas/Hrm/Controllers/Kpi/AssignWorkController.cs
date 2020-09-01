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
using AdminSoftware.Models;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class AssignWorkController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly CategoryKpiBll _categoryKpiBll;
        private readonly EmployeeBll _employeeBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;

        public AssignWorkController()
        {
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof (WorkDetailStatusEnum))
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

        public JsonResult AssignWorks(int status, DateTime fromDate, DateTime toDate)
        {
            var assignWorks = _assignWorkBll.GetAssignWorks(status, UserLogin.UserId, null, fromDate, toDate);
            return Json(assignWorks, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssignWork(string id)
        {
            var user = _userBll.GetUser(UserLogin.UserId);
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new AssignWork
                {
                    AssignWorkId = Guid.Empty.ToString(),
                    Status = (byte) WorkDetailStatusEnum.InPlan,
                    CreateBy = UserLogin.UserId,
                    CreateDate = DateTime.Now,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+7),
                    ActionCompany = user.DepartmentCompany
                });
            }
            return PartialView(_assignWorkBll.GetAssignWork(id));
        }

        public ActionResult Save(AssignWorkModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty}, JsonRequestBehavior.AllowGet);
                }
                if (model.DepartmentFisnishBy != null)
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "Công việc đã xác nhận hoàn thành không thể cập nhật"
                    }, JsonRequestBehavior.AllowGet);
                }
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
                model.CreateBy = UserLogin.UserId;
                model.CreateDate = DateTime.Now;
                if (string.IsNullOrEmpty(model.AssignWorkId) || model.AssignWorkId == Guid.Empty.ToString())
                {
                    model.AssignWorkId = Guid.NewGuid().ToString();
                    return Json(_assignWorkBll.Insert(model.ToObject())
                        ? new {Status = 1, Message = MessageAction.MessageCreateSuccess}
                        : new {Status = 0, Message = MessageAction.MessageActionFailed},
                        JsonRequestBehavior.AllowGet);
                }
                return Json(_assignWorkBll.Update(model.ToObject())
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

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return Json(new {Status = 0, Message = MessageAction.DataIsEmpty},
                        JsonRequestBehavior.AllowGet);
                }
                var assignWork = _assignWorkBll.GetAssignWork(id);
                if (assignWork.Status == (byte) WorkDetailStatusEnum.Finish)
                {
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message = "Công việc được giao đã hoàn thành không được xóa!"
                            },
                            JsonRequestBehavior.AllowGet);
                }
                if (_assignWorkBll.Delete(id))
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

        public ActionResult Employees(string id)
        {
            var employees = _employeeBll.GetEmployeesByDepartmentAndPosition(byte.Parse(id) == 2? (byte)DepartmentConpanyEnum.Department: (byte)DepartmentConpanyEnum.Employee,
                UserLogin.DepartmentId);
            return PartialView(employees);
        }

        public ActionResult Tasks()
        {
            ViewBag.CategoryKpis =
                _categoryKpiBll.GetCategoryKpis()
                    .Select(x => new KendoForeignKeyModel(x.KpiName, x.CategoryKpiId.ToString()));
            ViewBag.CategoryKpiId = UserLogin.CategoryKpiId ?? 0;
            return PartialView();
        }

        public JsonResult TaskResults(long? categoryKpiId)
        {
            if (categoryKpiId == null)
            {
                return Json(new List<Task>(), JsonRequestBehavior.AllowGet);
            }
            var tasks = _taskBll.GetTasks(null, true, categoryKpiId, null);
            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}