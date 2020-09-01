using System;
using System.Collections.Generic;
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
    public class WorkPlanController : BaseController
    {
        private readonly AutoNumberBll _autoNumberBll;
        private readonly DepartmentBll _departmentBll;
        private readonly EmployeeBll _employeeBll;
        private readonly HolidayBll _holidayBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;
        private readonly WorkPlanBll _workPlanBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly CategoryKpiBll _categoryKpiBll;
        public WorkPlanController()
        {
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _employeeBll = SingletonIpl.GetInstance<EmployeeBll>();
            _workPlanBll = SingletonIpl.GetInstance<WorkPlanBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _holidayBll = SingletonIpl.GetInstance<HolidayBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
        }

        private List<WorkPlanDetail> WorkPlanDetailsInMemory
        {
            get
            {
                if (Session["WorkPlanDetailsInMemory"] == null)
                    return new List<WorkPlanDetail>();
                return (List<WorkPlanDetail>)Session["WorkPlanDetailsInMemory"];
            }
            set { Session["WorkPlanDetailsInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkPointConfigs =
                _workPointConfigBll.GetWorkPointConfigs()
                    .Select(x => new KendoForeignKeyModel(x.WorkPointName, x.WorkPointConfigId.ToString()));
            ViewBag.Departments =
                _departmentBll.GetDepartments(null)
                    .Select(x => new KendoForeignKeyModel(x.DepartmentName, x.DepartmentId.ToString()));
            ViewBag.Users =
                _userBll.GetUsers(null)
                    .Select(x => new KendoForeignKeyModel(x.UserName, x.UserId.ToString()));
            ViewBag.Employee =
                _employeeBll.GetEmployees(null)
                    .Select(x => new KendoForeignKeyModel(x.FullName, x.EmployeeId.ToString()));
            return View();
        }

        public JsonResult WorkPlans(DateTime fromDate, DateTime toDate)
        {
            return Json(_workPlanBll.GetWorkPlansByUserId(UserLogin.UserId, fromDate, toDate),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult WorkPlan(string id)
        {
            WorkPlanDetailsInMemory = null;
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new WorkPlan
                {
                    CreateDate = DateTime.Now,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+7),
                    WorkPlanCode = _autoNumberBll.GetAutoNumber("KHCV"),
                    WorkPlanId = Guid.Empty.ToString(),
                    CreateBy = UserLogin.UserId
                });
            }
            var workPlan = _workPlanBll.GetWorkPlan(id);
            WorkPlanDetailsInMemory = workPlan.WorkPlanDetails;
            return PartialView(workPlan);
        }

        public ActionResult WorkPlanDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new WorkPlanDetail
                {
                    WorkPlanDetailId = Guid.Empty.ToString(),
                    WorkPlanId = Guid.Empty.ToString(),
                    Status = (byte)WorkDetailStatusEnum.InPlan,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+1),
                    Quantity = 1
                });
            }
            var workPlanDetails = WorkPlanDetailsInMemory;
            return PartialView(workPlanDetails.Find(x => x.WorkPlanDetailId == id));
        }

        public JsonResult WorkPlanDetails()
        {
            return Json(WorkPlanDetailsInMemory, JsonRequestBehavior.AllowGet);
        }

        public int Compare(DateTime firstDate, DateTime secondDate)
        {
            return DateTime.Compare(firstDate, secondDate);
        }

        public ActionResult Save(WorkPlanModel model)
        {
            try
            {
                if (!WorkPlanDetailsInMemory.Any() || model == null)
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

                var holidays = _holidayBll.GetHolidays(model.ToDate.Year, model.ToDate.Month);
                if (
                    holidays.Where(x => x.HolidayDate.ToString("yy-MM-dd") == model.ToDate.ToString("yy-MM-dd"))
                        .FirstOrDefault() != null)
                {
                    return Json(
                        new { Status = 0, Message = "Ngày kết thúc không được nằm trong ngày nghỉ!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (Compare(model.FromDate, model.ToDate) > 0)
                {
                    return Json(
                        new { Status = 0, Message = "Thời gian bắt đầu phải nhỏ hơn bằng thời gian hoàn thành!" },
                        JsonRequestBehavior.AllowGet);
                }

                if (
                    WorkPlanDetailsInMemory.Find(
                        x =>
                            Compare(DateTime.Parse(x.ToDate.ToShortDateString()),
                                DateTime.Parse(model.ToDate.ToShortDateString())) > 0) != null)
                {
                    return Json(
                        new
                        {
                            Status = 0,
                            Message = "Ngày kết thúc công việc không được lớn hơn ngày hoàn thành kế hoạch!"
                        },
                        JsonRequestBehavior.AllowGet);
                }
                if (
                    WorkPlanDetailsInMemory.Find(
                        x =>
                            DateTime.Parse(x.FromDate.ToShortDateString()) <
                            DateTime.Parse(model.FromDate.ToShortDateString())) != null)
                {
                    return Json(
                        new
                        {
                            Status = 0,
                            Message = "Ngày bắt đầu công việc không được nhỏ hơn ngày bắt đầu kế hoạch!"
                        },
                        JsonRequestBehavior.AllowGet);
                }
                var workPlan = model.ToObject();
                workPlan.WorkPlanDetails = WorkPlanDetailsInMemory;
                if (_workPlanBll.GetWorkPlanCheckDate(model.FromDate, model.WorkPlanId, workPlan.CreateBy) != null ||
                    _workPlanBll.GetWorkPlanCheckDate(model.ToDate, model.WorkPlanId, workPlan.CreateBy) != null)
                {
                    return Json(new { Status = 0, Message = "Trong thời gian này bạn đã có kế hoạch khác!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(workPlan.WorkPlanId) || workPlan.WorkPlanId == Guid.Empty.ToString())
                {
                    if (DateTime.Parse(workPlan.FromDate.ToShortDateString()) <
                        DateTime.Parse(DateTime.Now.ToShortDateString()))
                    {
                        return Json(
                            new { Status = 0, Message = "Ngày bắt đầu kế hoạch phải lớn hơn bằng ngày hiện tại!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    workPlan.ConfirmedDate = DateTime.Now;
                    workPlan.ConfirmedBy = workPlan.CreateBy;
                    workPlan.ApprovedDate = DateTime.Now;
                    workPlan.ApprovedBy = workPlan.CreateBy;
                    workPlan.WorkPlanId = Guid.NewGuid().ToString();
                    workPlan.CreateDate = DateTime.Now;
                    workPlan.WorkPlanCode = "KHCV";
                    var workPlanCode = "";
                    if (_workPlanBll.Insert(workPlan, ref workPlanCode))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                var workPlanCheck = _workPlanBll.GetWorkPlan(model.WorkPlanId);
                if (DateTime.Parse(workPlanCheck.FromDate.ToShortDateString()) >
                    DateTime.Parse(model.FromDate.ToShortDateString()))
                {
                    return
                        Json(
                            new
                            {
                                Status = 0,
                                Message = "Ngày bắt đầu kế hoạch phải lớn hơn bằng ngày bắt đầu kế hoạch khi tạo!"
                            },
                            JsonRequestBehavior.AllowGet);
                }
                if (!_workPlanBll.Update(workPlan))
                {
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveDetail(WorkPlanDetailModel model)
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
                var workPlanDetails = WorkPlanDetailsInMemory;
                var workPlanDetail = workPlanDetails.Find(x => x.WorkPlanDetailId == model.WorkPlanDetailId);
                var task = _taskBll.GetTask(model.TaskId);
                if (workPlanDetail == null)
                {
                    if (task == null)
                    {
                        return Json(new { Status = 0, Message = "Công việc không tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    workPlanDetails.Add(new WorkPlanDetail
                    {
                        WorkPlanDetailId = Guid.NewGuid().ToString(),
                        TaskId = model.TaskId,
                        ToDate =
                            DateTime.Parse(model.ToDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        FromDate =
                            DateTime.Parse(model.FromDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        Status = model.Status,
                        Description = model.Description,
                        Explanation = model.Explanation,
                        TaskName = task.TaskName,
                        TaskCode = task.TaskCode,
                        UsefulHourTask = task.UsefulHours,
                        Quantity = model.Quantity
                    });
                }
                else
                {
                    workPlanDetail.TaskId = model.TaskId;
                    workPlanDetail.ToDate =
                        DateTime.Parse(model.ToDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    workPlanDetail.TaskName = task.TaskName;
                    workPlanDetail.TaskCode = task.TaskCode;
                    workPlanDetail.FromDate =
                        DateTime.Parse(model.FromDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    workPlanDetail.Status = model.Status;
                    workPlanDetail.Description = model.Description;
                    workPlanDetail.Explanation = model.Explanation;
                    workPlanDetail.Quantity = model.Quantity;
                }
                WorkPlanDetailsInMemory = workPlanDetails;
                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Thêm công việc thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);

                if (_workPlanBll.Delete(id))
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

        public JsonResult DeleteDetail(string id)
        {
            try
            {
                var workPlanDetails = WorkPlanDetailsInMemory;
                var workPlanDetailDelete = workPlanDetails.Find(x => x.WorkPlanDetailId == id);
                if (workPlanDetailDelete.Status == (byte)WorkDetailStatusEnum.Finish)
                {
                    return
                        Json(
                            new
                            {
                                Status = 1,
                                Message = "Công việc đã hoàn thành không thể xóa"
                            },
                            JsonRequestBehavior.AllowGet);
                }
                workPlanDetails.Remove(workPlanDetailDelete);
                WorkPlanDetailsInMemory = workPlanDetails;

                return
                    Json(
                        new
                        {
                            Status = 1,
                            Message = "Xóa công việc thành công!"
                        },
                        JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
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


        public JsonResult ClearSession()
        {
            WorkPlanDetailsInMemory = null;
            return null;
        }
    }
}