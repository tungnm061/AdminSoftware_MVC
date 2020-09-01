using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using BusinessLogic.System;
using Core.Helper.Logging;
using Core.Singleton;
using AdminSoftware.Areas.System.Models.Kpi;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Hrm;
using Entity.Kpi;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class WorkStreamController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly AutoNumberBll _autoNumberBll;
        private readonly TaskBll _taskBll;
        private readonly UserBll _userBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly WorkStreamBll _workStreamBll;
        private readonly DepartmentBll _departmentBll;
        private readonly CategoryKpiBll _categoryKpiBll;
        public WorkStreamController()
        {
            _workStreamBll = SingletonIpl.GetInstance<WorkStreamBll>();
            _autoNumberBll = SingletonIpl.GetInstance<AutoNumberBll>();
            _userBll = SingletonIpl.GetInstance<UserBll>();
            _taskBll = SingletonIpl.GetInstance<TaskBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _categoryKpiBll = SingletonIpl.GetInstance<CategoryKpiBll>();
        }

        private List<WorkStreamDetail> WorkStreamDetailsInMemory
        {
            get
            {
                if (Session["WorkStreamDetailsInMemory"] == null)
                    return new List<WorkStreamDetail>();
                return (List<WorkStreamDetail>)Session["WorkStreamDetailsInMemory"];
            }
            set { Session["WorkStreamDetailsInMemory"] = value; }
        }

        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkPointConfigs =
                _workPointConfigBll.GetWorkPointConfigs()
                    .Select(x => new KendoForeignKeyModel(x.WorkPointName, x.WorkPointConfigId.ToString()));
            ViewBag.Users =
                _userBll.GetUsersOfEmployee()
                    .Select(x => new KendoForeignKeyModel(x.EmployeeCode + "-" + x.FullName, x.UserId.ToString()));
            return View();
        }

        public JsonResult WorkStreams(DateTime fromDate, DateTime toDate, int action)
        {
            return Json(_workStreamBll.GetWorkStreamsByUserId(UserLogin.UserId, fromDate, toDate, action),
                JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkStreamViewer(string id)
        {
            var workStream = _workStreamBll.GetWorkStream(id);
            return PartialView(workStream);
        }

        public JsonResult WorkStreamDetails()
        {
            return Json(WorkStreamDetailsInMemory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult WorkStreamDetail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new WorkStreamDetail
                {
                    WorkStreamDetailId = Guid.Empty.ToString(),
                    WorkStreamId = Guid.Empty.ToString(),
                    Status = 1,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+1),
                    IsDefault = false,
                    CreateBy = UserLogin.UserId,
                    CreateDate = DateTime.Now,
                    Quantity = 1
                });
            }
            return PartialView(WorkStreamDetailsInMemory.Find(x => x.WorkStreamDetailId == id));
        }

        public int Compare(DateTime firstDate, DateTime secondDate)
        {
            return DateTime.Compare(firstDate, secondDate);
        }
        public ActionResult WorkStream(string id)
        {
            WorkStreamDetailsInMemory = null;
            if (string.IsNullOrEmpty(id))
            {
                return PartialView(new WorkStream
                {
                    WorkStreamId = Guid.Empty.ToString(),
                    WorkStreamCode = _autoNumberBll.GetAutoNumber("VN"),
                    Status = 1,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(+7),
                    CreateDate = DateTime.Now,
                    CreateBy = UserLogin.UserId,
                    TaskId = Guid.Empty.ToString(),
                    AssignWorkId = Guid.Empty.ToString(),
                    ApprovedBy = null,
                    ApprovedDate = null
                });
            }
            var workStream = _workStreamBll.GetWorkStream(id);
            var performers = workStream.Performers;
            ViewBag.Performers = performers.Select(y =>
            {
                var firstOrDefault = _userBll.GetUsers(null).Where(x => x.UserId == y.PerformerBy).FirstOrDefault();
                return firstOrDefault != null
                    ? new KendoForeignKeyModel(firstOrDefault.UserName, y.PerformerBy.ToString())
                    : null;
            });

            WorkStreamDetailsInMemory = workStream.WorkStreamDetails;
            return PartialView(workStream);
        }

        public ActionResult Save(WorkStreamModel model)
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
                if (Compare(model.FromDate, model.ToDate) > 0)
                {
                    return Json(
                        new { Status = 0, Message = "Thời gian bắt đầu không được lớn hơn thời gian hoàn thành!" },
                        JsonRequestBehavior.AllowGet);
                }
                if (
                    WorkStreamDetailsInMemory.Find(
                        x =>
                            DateTime.Parse(x.ToDate.ToShortDateString()) >
                            DateTime.Parse(model.ToDate.ToShortDateString())) != null)
                {
                    return Json(
                        new
                        {
                            Status = 0,
                            Message = "Ngày kết thúc công việc không được lớn hơn ngày hoàn thành việc nhóm!"
                        },
                        JsonRequestBehavior.AllowGet);
                }
                if (
                    WorkStreamDetailsInMemory.Find(
                        x =>
                            DateTime.Parse(x.FromDate.ToShortDateString()) <
                            DateTime.Parse(model.FromDate.ToShortDateString())) != null)
                {
                    return Json(
                        new
                        {
                            Status = 0,
                            Message = "Ngày bắt đầu công việc không được nhỏ hơn ngày bắt đầu việc nhóm!"
                        },
                        JsonRequestBehavior.AllowGet);
                }

                var workStream = model.ToObject();

                workStream.WorkStreamDetails = WorkStreamDetailsInMemory;
                if (string.IsNullOrEmpty(workStream.WorkStreamId) || workStream.WorkStreamId == Guid.Empty.ToString())
                {
                    var workStreamDetail = new WorkStreamDetail
                    {
                        WorkStreamDetailId = Guid.NewGuid().ToString(),
                        Status = 1,
                        CreateBy = UserLogin.UserId,
                        CreateDate = DateTime.Now,
                        FromDate = workStream.FromDate,
                        ToDate = workStream.ToDate,
                        Description = workStream.Description,
                        IsDefault = true,
                        Quantity = 1,
                        VerifiedBy = UserLogin.UserId,
                        VerifiedDate = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        ApprovedFisnishBy = null,
                        ApprovedFisnishDate = null,
                        DepartmentFisnishBy = null,
                        DepartmentFisnishDate = null,
                        FisnishDate = null
                    };
                    if (workStream.TaskId != Guid.Empty.ToString())
                    {
                        workStreamDetail.TaskId = workStream.TaskId;
                    }
                    if (workStream.AssignWorkId != Guid.Empty.ToString())
                    {
                        var assignWork = _assignWorkBll.GetAssignWork(model.AssignWorkId);
                        workStreamDetail.TaskId = assignWork.TaskId;
                    }
                    var workStreamDetails = WorkStreamDetailsInMemory;
                    workStreamDetails.Add(workStreamDetail);
                    WorkStreamDetailsInMemory = workStreamDetails;
                    workStream.WorkStreamDetails = WorkStreamDetailsInMemory;
                    workStream.WorkStreamCode = "VN";
                    workStream.CreateDate = DateTime.Now;
                    workStream.WorkStreamId = Guid.NewGuid().ToString();
                    workStream.ApprovedBy = null;
                    workStream.ApprovedDate = null;
                    var performersNew = model.PerformerBys.Select(item => new Performer
                    {
                        WorkStreamId = workStream.WorkStreamId,
                        PerformerBy = Int32.Parse(item)
                    }).ToList();
                    workStream.Performers = performersNew;
                    var workStreamCode = "";
                    if (_workStreamBll.Insert(workStream, ref workStreamCode))
                    {
                        return Json(new { Status = 1, Message = MessageAction.MessageCreateSuccess },
                            JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                        JsonRequestBehavior.AllowGet);
                }
                var performers = model.PerformerBys.Select(item => new Performer
                {
                    WorkStreamId = workStream.WorkStreamId,
                    PerformerBy = Int32.Parse(item)
                }).ToList();
                workStream.Performers = performers;
                if (workStream.ApprovedBy != null)
                {
                    foreach (var item in workStream.WorkStreamDetails)
                    {
                        item.VerifiedBy = UserLogin.UserId;
                        item.VerifiedDate =
                            DateTime.Parse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    }
                }
                if (_workStreamBll.Update(workStream))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logging.PutError(ex.Message, ex);
                return Json(new { Status = 0, ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveDetail(WorkStreamDetailModel model)
        {
            try
            {
                if (model == null)
                {
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
                }
                var workStreamDetails = WorkStreamDetailsInMemory;
                var workStreamDetail = workStreamDetails.Find(x => x.WorkStreamDetailId == model.WorkStreamDetailId);

                if (model.IsDefault)
                {
                    return Json(new { Status = 0, Message = "Công việc này không được cập nhật" },
                        JsonRequestBehavior.AllowGet);
                }
                if (model.DepartmentFisnishBy != null)
                {
                    return Json(new
                    {
                        Status = 0,
                        Message = "Công việc đã xác nhận hoàn thành không thể cập nhật"
                    }, JsonRequestBehavior.AllowGet);
                }
                if (workStreamDetail == null)
                {
                    var task = _taskBll.GetTask(model.TaskId);
                    if (task == null)
                    {
                        return Json(new { Status = 0, Message = "Công việc không tồn tại trong hệ thống!" },
                            JsonRequestBehavior.AllowGet);
                    }
                    workStreamDetails.Add(new WorkStreamDetail
                    {
                        WorkStreamDetailId = Guid.NewGuid().ToString(),
                        TaskId = model.TaskId,
                        ToDate =
                            DateTime.Parse(model.ToDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        FromDate =
                            DateTime.Parse(model.FromDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        Status = model.Status,
                        Description = model.Description,
                        CreateBy = model.CreateBy,
                        CreateDate = model.CreateDate,
                        TaskCode = model.TaskCode,
                        TaskName = model.TaskName,
                        CreateByName = UserLogin.EmployeeName,
                        CreateByCode = UserLogin.EmployeeCode,
                        DepartmentName = UserLogin.DepartmentName,
                        VerifiedBy = null,
                        VerifiedDate = null,
                        //VerifiedDate = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt")),
                        IsDefault = false,
                        Quantity = model.Quantity
                    });
                }
                else
                {
                    workStreamDetail.TaskId = model.TaskId;
                    workStreamDetail.ToDate =
                        DateTime.Parse(model.ToDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    workStreamDetail.FromDate =
                        DateTime.Parse(model.FromDate.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    workStreamDetail.Status = model.Status;
                    workStreamDetail.CreateBy = model.CreateBy;
                    workStreamDetail.Description = model.Description;
                    workStreamDetail.CreateDate = model.CreateDate;
                    workStreamDetail.IsDefault = model.IsDefault;
                    workStreamDetail.TaskCode = model.TaskCode;
                    workStreamDetail.TaskName = model.TaskName;
                    workStreamDetail.CreateByName = UserLogin.FullName;
                    workStreamDetail.VerifiedDate = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToString("hh:mm tt"));
                    workStreamDetail.VerifiedBy = model.CreateBy;
                    workStreamDetail.Quantity = model.Quantity;
                }
                WorkStreamDetailsInMemory = workStreamDetails;
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


        public ActionResult AssignWorkSearch()
        {
            return PartialView(_assignWorkBll.GetAssignWorks(null, null, UserLogin.UserId, null, null));
        }

        public JsonResult DeleteDetail(string id)
        {
            try
            {
                var workStreamDetails = WorkStreamDetailsInMemory;
                workStreamDetails.Remove(workStreamDetails.Find(x => x.WorkStreamDetailId == id));
                WorkStreamDetailsInMemory = workStreamDetails;
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

        public JsonResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return Json(new { Status = 0, Message = MessageAction.DataIsEmpty },
                        JsonRequestBehavior.AllowGet);

                if (_workStreamBll.Delete(id))
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
            WorkStreamDetailsInMemory = null;
            return null;
        }
    }
}