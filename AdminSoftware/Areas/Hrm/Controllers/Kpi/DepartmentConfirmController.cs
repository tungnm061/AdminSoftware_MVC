using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using BusinessLogic.Hrm;
using BusinessLogic.Kpi;
using Core.Enum;
using Core.Singleton;
using AdminSoftware.Controllers;
using AdminSoftware.Models;
using Entity.Kpi;
using Newtonsoft.Json;

namespace AdminSoftware.Areas.Hrm.Controllers.Kpi
{
    public class DepartmentConfirmController : BaseController
    {
        private readonly AssignWorkBll _assignWorkBll;
        private readonly DepartmentBll _departmentBll;
        private readonly SuggesWorkBll _suggesWorkBll;
        private readonly WorkDetailBll _workDetailBll;
        private readonly WorkPlanDetailBll _workPlanDetailBll;
        private readonly WorkPointConfigBll _workPointConfigBll;
        private readonly WorkStreamDetailBll _workStreamDetailBll;
        public DepartmentConfirmController()
        {
            _departmentBll = SingletonIpl.GetInstance<DepartmentBll>();
            _workPlanDetailBll = SingletonIpl.GetInstance<WorkPlanDetailBll>();
            _workStreamDetailBll = SingletonIpl.GetInstance<WorkStreamDetailBll>();
            _assignWorkBll = SingletonIpl.GetInstance<AssignWorkBll>();
            _suggesWorkBll = SingletonIpl.GetInstance<SuggesWorkBll>();
            _workDetailBll = SingletonIpl.GetInstance<WorkDetailBll>();
            _workPointConfigBll = SingletonIpl.GetInstance<WorkPointConfigBll>();
        }
        [HrmFilter]
        public ActionResult Index()
        {
            ViewBag.WorkPointType = from WorkPointType s in Enum.GetValues(typeof(WorkPointType))
                                    let singleOrDefault =
                                        (DescriptionAttribute)
                                            s.GetType()
                                                .GetField(s.ToString())
                                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                .SingleOrDefault()
                                    where singleOrDefault != null
                                    select
                                        new KendoForeignKeyModel { value = singleOrDefault.Description, text = singleOrDefault.Description };
            ViewBag.WorkDetailStatusEnum = from WorkDetailStatusEnum s in Enum.GetValues(typeof(WorkDetailStatusEnum))
                                           let singleOrDefault =
                                               (DescriptionAttribute)
                                                   s.GetType()
                                                       .GetField(s.ToString())
                                                       .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                       .SingleOrDefault()
                                           where singleOrDefault != null
                                           select new KendoForeignKeyModel { value = ((byte)s).ToString(), text = singleOrDefault.Description };

            ViewBag.StatusWorkDetail = from StatusWorkDetail s in Enum.GetValues(typeof(StatusWorkDetail))
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
        public JsonResult Save(string id, int action, int workType, string workPointType)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (workDetail == null)
            {
                return Json(new { Status = 0, Message = MessageAction.DataIsEmpty }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.ApprovedFisnishBy != null)
            {
                return Json(new { Status = 0, Message = "Công việc đã được TPHCNS xác nhận k thể cập nhật!" }, JsonRequestBehavior.AllowGet);
            }
            workDetail.WorkPointType = workPointType;
            var workPointConfig = _workPointConfigBll.GetWorkPointConfig(workDetail.WorkPointConfigId);
            switch (workDetail.WorkPointType)
            {
                case "A":
                    workDetail.WorkPoint = workPointConfig.WorkPointA;
                    break;
                case "B":
                    workDetail.WorkPoint = workPointConfig.WorkPointB;
                    break;
                case "C":
                    workDetail.WorkPoint = workPointConfig.WorkPointC;
                    break;
                case "D":
                    workDetail.WorkPoint = workPointConfig.WorkPointD;
                    break;
            }
            if (action == 1)
            {
                workDetail.Status = 3;
                workDetail.DepartmentFisnishBy = UserLogin.UserId;
                workDetail.DepartmentFisnishDate = DateTime.Now;
            }
            if (action == 2)
            {
                workDetail.Status = 5;
                workDetail.DepartmentFisnishBy = UserLogin.UserId;
                workDetail.DepartmentFisnishDate = DateTime.Now;
                workDetail.WorkPoint = null;
                workDetail.WorkPointType = null;
            }
            if (action != 1 && action != 2)
            {
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkPlanDetail)
            {
                var workPlanDetail = _workPlanDetailBll.GetWorkPlanDetail(workDetail.WorkDetailId);
                workPlanDetail.Status = workDetail.Status;
                workPlanDetail.DepartmentFisnishBy = workDetail.DepartmentFisnishBy;
                workPlanDetail.DepartmentFisnishDate = workDetail.DepartmentFisnishDate;
                workPlanDetail.WorkPointType = workDetail.WorkPointType;
                workPlanDetail.WorkPoint = workDetail.WorkPoint;
                if (_workPlanDetailBll.Update(workPlanDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.AssignWork)
            {
                var assignWork = _assignWorkBll.GetAssignWork(workDetail.WorkDetailId);
                assignWork.Status = (byte)workDetail.Status;
                assignWork.DepartmentFisnishBy = workDetail.DepartmentFisnishBy;
                assignWork.DepartmentFisnishDate = workDetail.DepartmentFisnishDate;
                assignWork.WorkPointType = workDetail.WorkPointType;
                assignWork.WorkPoint = workDetail.WorkPoint;
                if (_assignWorkBll.Update(assignWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.WorkStreamDetail)
            {
                var workStreamDetail = _workStreamDetailBll.GetWorkStreamDetail(workDetail.WorkDetailId);
                workStreamDetail.Status = workDetail.Status;
                workStreamDetail.DepartmentFisnishBy = workDetail.DepartmentFisnishBy;
                workStreamDetail.DepartmentFisnishDate = workDetail.DepartmentFisnishDate;
                workStreamDetail.WorkPointType = workDetail.WorkPointType;
                workStreamDetail.WorkPoint = workDetail.WorkPoint;
                if (_workStreamDetailBll.Update(workStreamDetail))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            if (workDetail.WorkType == (int)StatusWorkDetail.SuggesWork)
            {
                var suggesWork = _suggesWorkBll.GetSuggesWork(workDetail.WorkDetailId);
                suggesWork.Status = (byte)workDetail.Status;
                suggesWork.DepartmentFisnishBy = workDetail.DepartmentFisnishBy;
                suggesWork.DepartmentFisnishDate = workDetail.DepartmentFisnishDate;
                suggesWork.WorkPointType = workDetail.WorkPointType;
                suggesWork.WorkPoint = workDetail.WorkPoint;
                if (_suggesWorkBll.Update(suggesWork))
                {
                    return Json(new { Status = 1, Message = MessageAction.MessageUpdateSuccess },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Status = 0, Message = MessageAction.MessageActionFailed }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult WorkDetails(int action, DateTime? fromDate, DateTime? toDate)
        {
            var department = _departmentBll.GetDepartment(UserLogin.DepartmentId ?? 0);
            if (department == null || string.IsNullOrEmpty(department.Path))
            {
                return
                Json(_workDetailBll.GetWorkDetailsByPath("", action, fromDate, toDate),
                    JsonRequestBehavior.AllowGet);
            }
            return Json(_workDetailBll.GetWorkDetailsByPath(department.Path, action, fromDate, toDate), JsonRequestBehavior.AllowGet);
        }

        

        public ActionResult ConfirmFinish(string id, int workType)
        {
            var workingNotes = new List<WorkingNote>();
            var workDetail = _workDetailBll.GetWorkDetail(id, workType);

            if (workDetail.WorkingNote != null)
            {
                workingNotes = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            ViewBag.WorkingNotes = workingNotes;
            return PartialView(workDetail);
        }

        public ActionResult CancelFinish(string id, int workType)
        {
            var workingNotes = new List<WorkingNote>();
            var explanations = new List<Explanation>();

            var workDetail = _workDetailBll.GetWorkDetail(id, workType);
            if (!string.IsNullOrEmpty(workDetail.WorkingNote))
            {
                workingNotes = JsonConvert.DeserializeObject<List<WorkingNote>>(workDetail.WorkingNote);
            }
            if (!string.IsNullOrEmpty(workDetail.Explanation))
            {
                explanations = JsonConvert.DeserializeObject<List<Explanation>>(workDetail.Explanation);
            }
            ViewBag.WorkingNotes = workingNotes;
            ViewBag.Explanations = explanations;
            return PartialView(workDetail);
        }

        public JsonResult ClearSession()
        {
            return null;
        }
    }
}