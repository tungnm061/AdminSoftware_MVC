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
using Entity.Hrm;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class SuggesWorkController : BaseController
    {
        private readonly DepartmentBll _departmentBll;
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly TaskBll _taskBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly CategoryKpiBll _categoryKpiBll;

        public SuggesWorkController()
        {
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkPointConfigs =
                _workPointConfigBll.GetWorkPointConfigs()
                    .Select(x => new KendoForeignKeyModel(x.WorkPointName, x.WorkPointConfigId.ToString()));
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof(WorkDetailStatusEnum))
                                           let singleOrDefault =
                                               (DescriptionAttribute)
                                                   s.GetType()
                                                       .GetField(s.ToString())
                                                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                       .SingleOrDefault()
                                           where singleOrDefault != null
                                           select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            return View();
        }

        public JsonResult SuggesWorks(DateTime fromDate, DateTime toDate, int action)
        {
            if (action == 10)
            {
                return Json(_suggesWorkBll.GetSuggesWorks(UserLogin.UserId, 10, fromDate, toDate), JsonRequestBehavior.AllowGet);

            }
            return Json(_suggesWorkBll.GetSuggesWorks(UserLogin.UserId, 11, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SuggesWork(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new SuggesWork
                {
                    SuggesWorkId = Guid.Empty.ToString(),
                    CreateBy = UserLogin.UserId,
                    CreateDate = DateTime.Now,
                    Status = (byte)WorkDetailStatusEnum.InPlan,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+1),
                    Quantity = 1
                });
            }
            return PartialView(_suggesWorkBll.GetSuggesWork(id));
        }

        public int Compare(DateTime firstDate, DateTime secondDate)
        {
            return DateTime.Compare(firstDate, secondDate);
        }

        public JsonResult Save(SuggesWorkModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
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
                if (model.DepartmentFisnishBy != null)
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "Công việc đã xác nhận hoàn thành không thể cập nhật"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.SuggesWorkId) || model.SuggesWorkId == Guid.Empty.ToString())
                {
                    model.CreateBy = UserLogin.UserId;
                    model.DepartmentId = UserLogin.DepartmentId ?? 0;
                    model.SuggesWorkId = Guid.NewGuid().ToString();
                    model.VerifiedDate = DateTime.Now;
                    model.VerifiedBy = UserLogin.UserId;
                    return Json(_suggesWorkBll.Insert(model.ToObject())
                        ? new { Status = 1, Message = MessageAction.MessageCreateSuccess }
                        : new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(_suggesWorkBll.Update(model.ToObject())
                    ? new { Status = 1, Message = MessageAction.MessageUpdateSuccess }
                    : new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TaskSearch()
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

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);
                if (_suggesWorkBll.Delete(id))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageDeleteSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}